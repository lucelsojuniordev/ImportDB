using System;
using FirebirdSql.Data.FirebirdClient;
using System.Windows.Forms;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using SyncDB.SQL;
using SyncDB.Conect;
using SyncDB.Models;
using ImportDB;

namespace SyncDB
{
    public partial class ImportDB : Form
    {
        private string stringConexao { get; set; }
        private FbConnection conn { get; set; }
        private ConnApp envio { get; set; }
        private GerarSQL consultas { get; set; }
        public Usuario usuario { get; set; }
        public string msgEnvio { get; set; }
        public string cnpj { get; set; }
        public bool concluido = false;
        public bool conectou = false;
        
        public ImportDB()
        {
            InitializeComponent();
            GerarSQL();
        }
        private void btnConectar_Click(object sender, EventArgs e)
        {          

            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Program.als"))
            {
                MessageBox.Show("Arquivo Program.als não encontrado");
                return;
            }
                        
            string[] arq = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Program.als").ToString().Split(':');
            stringConexao = String.Format(@"User=analistats;Password=wvaebxti;Database={0};DataSource={1};Pooling=False;Connection timeout=25", arq[1] + ":" + arq[2].Split(';')[0], arq[0]);
            conn = new FbConnection { ConnectionString = stringConexao };

            try
            {
                conn.Open();
             
            }
            catch 
            {
                MessageBox.Show("Não foi possivel Conectar ao Banco de Dados. Verique : " + arq[1] + ":" + arq[2].Split(';')[0], arq[0]);
                return;
            }


            if (conn.State.ToString() == "Open")
            {
                btnConectar.Text = "Conectado";
                conectou = true;
                concluido = false;
                btnImportar.Text = "Importar Dados";
            }
            else 
            {
                MessageBox.Show("Não foi possível Conectar a Base de Dados");
                return;
            }
            conn.Close();

        }
        private void btnGerar_Click(object sender, EventArgs e)
        {
            if (concluido | !conectou)
            {
                Application.Exit();
            }

            try
            {
                conn = new FbConnection { ConnectionString = stringConexao };
                envio = new ConnApp();
                consultas = new GerarSQL();
                conn.Open();
                
                //Preparar para importar dados
                FbCommand cmd = new FbCommand(consultas.Empresa(), conn);
                string jsonEmpresa = JsonConvert.SerializeObject(RetornaDT(cmd), Formatting.None).ToString().Replace("[{", "{").Replace("}]", "}");
                DataTable dt = RetornaDT(cmd);
                string cnpj = dt.Rows[0].Field<String>("CPF_CNPJ").ToString();
                string token = envio.ExisteEmpresa(cnpj);
                //Consulta Empresa
                if (token != null)
                {
                    if (MessageBox.Show(" ************************ AVISO *********************** \n\nEssa empresa já está cadastrada em nosso sistema.\n\nSe deseja refazer a importação seus dados serão APAGADOS para dar inicio a esse novo processo.\n\nEscolha SIM e depois confirme a contra-senha para continuar. ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        btnImportar.Text = "SAIR";
                        btnConectar.Text = "Conectar";
                        concluido = true;
                        conectou = false;
                        return;
                    }
                    else
                    {
                        Senha senha = new Senha();
                        senha.ShowDialog();
                        envio.ApagarEmpresa(token);
                    }
                   
                }
                //Cadastra Empresa
                var retorno = envio.CadastraEmpresa(jsonEmpresa);
                usuario = JsonConvert.DeserializeObject<Usuario>(retorno);
            }
            catch (Exception )
            {   
                MessageBox.Show("Verifique os dados da Empresa");
                return;
            }

            //Erro no cadastro da Empresa
            if (usuario == null) 
            {
                MessageBox.Show("Não foi possível cadastrar a Empresa");
                return;
            }

            //Cadastrar Dados pelos sqls na pasta
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\SQL");
            foreach (FileInfo file in dir.GetFiles())
            {
                string tabela = file.Name.Replace(".sql", "");
                string sql = File.ReadAllText(file.FullName);
                DataTable data = RetornaDT(new FbCommand(sql, conn));
                int enviados = 0;
                int total = data.Rows.Count;
                int qtdenvios = 10;
                int qntlotes = (total % qtdenvios == 0) ? (total/qtdenvios) : (total / qtdenvios) +1;
                int enviando = 0;
                string msg = "Total de registro da tabela " + tabela.ToString() + " enviados: " + total.ToString() + "\n";
                while (enviados < total)
                {
                    enviando = (enviados + qtdenvios);
                    for (; enviados < enviando; enviados = (enviados + qtdenvios)) 
                    {
                        string lote = JsonConvert.SerializeObject(RetornaDT(new FbCommand(String.Format(sql + "rows {0} to {1}", enviados+1, enviando), conn)));
                        StreamWriter salvar = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\JSON\\DadosEnviados - " + tabela + enviados.ToString() + ".txt");
                        salvar.WriteLine(envio.CadastarDados(lote, usuario.TokenAPI, tabela));
                        salvar.Close();
                        btnConectar.Text = tabela + @":" + enviados.ToString() + @"/" + total.ToString();
                    }
                    
                }
              
                msgEnvio = msgEnvio + msg;
                btnConectar.Text = tabela + @":" + enviados.ToString() + @"/" + total.ToString();
            }
                        
            MessageBox.Show("Importação Concluída\n" + msgEnvio );
            conn.Close();
            concluido = true;
            btnConectar.Text = "Importação Concluída";
            btnImportar.Text = "SAIR";

        }

        private DataTable RetornaDT(FbCommand cmdComando)
        {
            FbDataAdapter daAdaptador = new FbDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                cmdComando.Connection = conn;
                daAdaptador.SelectCommand = cmdComando;
                daAdaptador.Fill(dt);
                return dt;
            }
            catch (FbException ex)
            {
                throw ex;
            }

        }
                        
        private void GerarSQL() {

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\SQL"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\SQL");
            }
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\JSON"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\JSON");
            }
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Retornos"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Retornos");
            }
            GerarSQL FonteSQL = new GerarSQL();

            using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\SQL\\produto.sql"))
            {
                sw.WriteLine(FonteSQL.Produtos());
                sw.Close();
            }

            using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\SQL\\favorecido.sql"))
            {
                sw.WriteLine(FonteSQL.Favorecidos());
                sw.Close();
            }

        }

    }
}
