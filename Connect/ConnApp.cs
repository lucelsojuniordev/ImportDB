using System.Net;
using System.IO;
using System.Text;
using System;

namespace SyncDB.Conect
{
    class ConnApp
    {
        const string IPServer = @"http://201.30.93.50/";
        const string URIempresa = @"importacao/empresa";

        public string CadastraEmpresa(string obj)
        {
            try
            {
                string resultado = string.Empty;
                HttpWebRequest req = WebRequest.CreateHttp(IPServer + URIempresa);
                req.Method = "POST";
                req.Accept = "application/json";
                req.ContentType = "application/json";
                var conteudo = Encoding.UTF8.GetBytes(obj);
                req.ContentLength = conteudo.Length;

                using (var stream = req.GetRequestStream())
                {
                    stream.Write(conteudo, 0, conteudo.Length);
                    stream.Close();
                }

                using (var retorno = req.GetResponse())
                {
                    var streamDados = retorno.GetResponseStream();
                    StreamReader reader = new StreamReader(streamDados);
                    object objResponse = reader.ReadToEnd();
                    string postRetorno = objResponse.ToString();
                    resultado = postRetorno;
                    StreamWriter salvar = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Retornos\RetornoEmpresa.txt");
                    salvar.WriteLine(postRetorno);
                    salvar.Close();
                    retorno.Close();
                }
                return resultado;
            }
            catch (Exception e)
            {
                return e.Message;
            }


        }
        public string CadastarDados(string obj, string token, string URI)
        {
            try
            {
                string resultado = string.Empty;
                HttpWebRequest req = WebRequest.CreateHttp(IPServer + @"importacao/" + URI);
                req.Method = "POST";
                req.Accept = "application/json";
                req.ContentType = "application/json";
                req.Headers.Add("Authorization", "Bearer " + token);

                var conteudo = Encoding.UTF8.GetBytes(obj);
                req.ContentLength = conteudo.Length;

                using (var stream = req.GetRequestStream())
                {
                    stream.Write(conteudo, 0, conteudo.Length);
                    stream.Close();
                }

                using (var retorno = req.GetResponse())
                {
                    var streamDados = retorno.GetResponseStream();
                    StreamReader reader = new StreamReader(streamDados);
                    object objResponse = reader.ReadToEnd();
                    string postRetorno = objResponse.ToString();
                    resultado = postRetorno;
                    retorno.Close();
                }
                return resultado;
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }
        public string ExisteEmpresa(string cnpj) 
        {
            string responseFromServer = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(IPServer + @"importacao/existe/" + cnpj);
                WebResponse response = request.GetResponse();
                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    string completo = reader.ReadToEnd();
                    responseFromServer = completo.Substring(33, 236);
                }
                return responseFromServer;
               
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string ApagarEmpresa( string token)
        {
            try
            {
                HttpWebRequest req = WebRequest.CreateHttp(IPServer + @"importacao/zerar/" + token);
                req.Method = "DELETE";
                req.Accept = "application/json";
                req.ContentType = "application/json";
                req.Headers.Add("Authorization", "Bearer " + token);
                return token;
            }
            catch (Exception e)
            {
                return token;
            }
        }
    }
}
