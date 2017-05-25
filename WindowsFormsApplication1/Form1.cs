using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        int nPasso = -1;
        bool VelocidadeRapida = true;


        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://indigo.rafson.com.br/");

        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            var script = webBrowser1.Document.CreateElement("script");

            script.SetAttribute("type", "text/javascript");
            var alertBlocker = "window.alert = function () { }; window.confirm=function () { }; ";
            script.SetAttribute("text", alertBlocker);
            webBrowser1.Document.GetElementsByTagName("head")[0].AppendChild(script);



            var script2 = webBrowser1.Document.CreateElement("script");
            script2.SetAttribute("type", "text/javascript");
            var GetFrame = @"
            function GetConteudoIframe(){ 
               var iFrame = document.getElementsByTagName('iframe')[0];
                        var iFrameBody;
                        if (iFrame.contentDocument)
                        { // FF
                            iFrameBody = iFrame.contentDocument.getElementsByTagName('body')[0];
                        }
                        else if (iFrame.contentWindow)
                        { // IE
                            iFrameBody = iFrame.contentWindow.document.getElementsByTagName('body')[0];
                        }
                        return iFrameBody.innerHTML;
        
                } ";


            script2.SetAttribute("text", GetFrame);
            webBrowser1.Document.GetElementsByTagName("head")[0].AppendChild(script2);



            var script3 = webBrowser1.Document.CreateElement("script");
            script3.SetAttribute("type", "text/javascript");
            var visibleResult = @"function visibleResult()
            {
                window.document.all['divResult'].style.display = 'block';
                window.document.all['divResult'].innerText = 'Um texto';
                var numero1 = parseInt(document.form1.numero1.value);
                var numero2 = parseInt(document.form1.numero2.value);

                var somaEntrada = parseInt(document.form1.soma.value);
                var subtracaoEntrada = parseInt(document.form1.subtracao.value);

                var soma = numero1 + numero2;
                var subtracao = numero1 - numero2;

                var textoResposta='';


                if(soma==somaEntrada && subtracao==subtracaoEntrada)
                {
	                textoResposta= 'O Primeiro numero é: ' + numero1 + '\nO Segundo numero é:' + numero2 + '\nA soma é:' + soma + '\nA Subtração é ' + subtracao + '\nOs resultados que você colocou estão corretos';
	     
                }
                else{
	                textoResposta = 'O resultado não está certo. você deve fazer a Soma e a Subtração e colocar nos lugares corretos';

                }
                

                window.document.all['divResult'].innerText = textoResposta;
                return 'ok';
            }
            ";
            script3.SetAttribute("text", visibleResult);
            webBrowser1.Document.GetElementsByTagName("head")[0].AppendChild(script3);

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (nPasso == 0)
            {


                webBrowser1.Document.GetElementById("login").SetAttribute("value", "rafson.silva");
                webBrowser1.Document.GetElementById("Password").SetAttribute("value", "indigo.2017");
                webBrowser1.Document.GetElementById("Submit").InvokeMember("click");

                nPasso++;

            }
            else if (nPasso == 1)
            {
                webBrowser1.Navigate("http://indigo.rafson.com.br/01.php");

                nPasso++;

            }
            else if (nPasso == 2)
            {
                Regex Grid = new Regex("<tr>[^<]*<td +[^>]*>(?<codigo>[^<]*)</td>[^<]*<td +[^>]*>(?<nome>[^<]*)</td>[^<]*<td +[^>]*>(?<apelido>[^<]*)</td>[^<]*<td +[^>]*>(?<trabalho>[^<]*)</td>[^<]*<td +[^>]*>(?<email>[^<]*)</td>[^<]*</tr>", RegexOptions.IgnoreCase);
                var MatchGrid = Grid.Matches(webBrowser1.Document.Body.InnerHtml);
                List<Grid1> ListaGrid1 = new List<Grid1>();
                foreach (var M in MatchGrid.Cast<Match>())
                {
                    ListaGrid1.Add(new Grid1()
                    {
                        codigo = M.Groups["codigo"].Value,
                        nome = M.Groups["nome"].Value,
                        apelido = M.Groups["apelido"].Value,
                        trabalho = M.Groups["trabalho"].Value,
                        email = M.Groups["email"].Value,
                    });
                }

                webBrowser1.Document.GetElementById("login").SetAttribute("value", ListaGrid1.Count().ToString());
                webBrowser1.Document.GetElementsByTagName("textarea")[0].SetAttribute("value", string.Join("\n", ListaGrid1.OrderBy(a => a.codigo).Select(a => a.codigo)));
                webBrowser1.Document.GetElementsByTagName("textarea")[1].SetAttribute("value", string.Join("\n", ListaGrid1.OrderBy(a => a.apelido).Select(a => a.apelido)));

                webBrowser1.Document.GetElementById("Submit").InvokeMember("click");


                nPasso++;

            }
            else if (nPasso == 3)
            {
                if (!VelocidadeRapida)
                    Thread.Sleep(5000);

                webBrowser1.Navigate("http://indigo.rafson.com.br/02.php");

                nPasso++;

            }
            else if (nPasso == 4)
            {
                var vHtml = (string)webBrowser1.Document.InvokeScript("GetConteudoIframe");

                Regex Regex02frameResponse = new Regex("<p>(?<texto>[^<]*)</p>", RegexOptions.IgnoreCase);
                var Match02frameResponse = Regex02frameResponse.Match(vHtml);
                if (!Match02frameResponse.Success)
                    throw new Exception("");
                var texto2 = Match02frameResponse.Groups["texto"].Value;

                webBrowser1.Document.GetElementsByTagName("textarea")[0].SetAttribute("value", texto2);

                webBrowser1.Document.GetElementById("Submit").InvokeMember("click");
                nPasso++;
            }
            else if (nPasso == 5)
            {
                if (!VelocidadeRapida)
                    Thread.Sleep(5000);

                webBrowser1.Navigate("http://indigo.rafson.com.br/03.php");


                nPasso++;
            }
            else if (nPasso == 6)
            {
                if (e.Url.AbsolutePath != "/03.php")
                {
                    webBrowser1.Navigate("http://indigo.rafson.com.br/03.php");
                    return;
                }

                webBrowser1.Document.GetElementById("NomeCompleto").SetAttribute("value", "Henrique Gomes Victorino");
                webBrowser1.Document.GetElementById("Email").SetAttribute("value", "henriquevictorino@gmail.com");


                HtmlElementCollection col = webBrowser1.Document.GetElementsByTagName("input");

                int nCheck = 0;

                foreach (HtmlElement item in col)
                {
                    if (item.OuterHtml.Contains("checkbox"))
                    {
                        nCheck++;
                        if (nCheck == 2)
                            item.InvokeMember("CLICK");
                        else if (nCheck == 3)
                            item.InvokeMember("CLICK");
                        else if (nCheck == 6)
                            item.InvokeMember("CLICK");
                    }
                }
                nCheck = 0;


                foreach (HtmlElement item in col)
                {
                    if (item.OuterHtml.Contains("radio"))
                    {
                        nCheck++;
                        if (nCheck == 1)
                            item.InvokeMember("CLICK");

                    }
                }

                HtmlElement sel = webBrowser1.Document.GetElementById("sel1");

                sel.GetElementsByTagName("option")[4].SetAttribute("selected", "selected");

                webBrowser1.Document.GetElementById("Submit").InvokeMember("click");

                nPasso++;
            }
            else if (nPasso == 7)
            {
                if (!VelocidadeRapida)
                    Thread.Sleep(5000);

                webBrowser1.Navigate("http://indigo.rafson.com.br/04.php");
                nPasso++;
            }

            else if (nPasso == 8)
            {

                timer1.Enabled = true;

            }
            else if (nPasso == 9)
            {
                if (!VelocidadeRapida)
                    Thread.Sleep(5000);

                webBrowser1.Navigate("http://indigo.rafson.com.br/05.php");
                nPasso++;

            }
            else if (nPasso == 10)
            {
                if (e.Url.AbsolutePath != "/05.php")
                {
                    webBrowser1.Navigate("http://indigo.rafson.com.br/05.php");
                    return;
                }
                Thread.Sleep(1000);

                webBrowser1.Document.GetElementById("Submit").InvokeMember("click");

                nPasso++;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            int numero1 = Convert.ToInt32(webBrowser1.Document.GetElementById("numero1").GetAttribute("value"));
            int numero2 = Convert.ToInt32(webBrowser1.Document.GetElementById("numero2").GetAttribute("value"));

            webBrowser1.Document.GetElementById("soma").SetAttribute("value", (numero1 + numero2).ToString());
            webBrowser1.Document.GetElementById("subtracao").SetAttribute("value", (numero1 - numero2).ToString());

            webBrowser1.Document.GetElementById("Operacao").InvokeMember("click");
            var vHtml = (string)webBrowser1.Document.InvokeScript("visibleResult");



            timer1.Enabled = false;

            nPasso++;

            webBrowser1.Document.GetElementById("Submit").InvokeMember("click");

        }

        class Grid1
        {
            public string codigo { get; set; }
            public string nome { get; set; }
            public string apelido { get; set; }
            public string trabalho { get; set; }
            public string email { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            nPasso = 0;
            VelocidadeRapida = true;
            webBrowser1.Navigate("http://indigo.rafson.com.br/");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            nPasso = 0;
            VelocidadeRapida = false;
            webBrowser1.Navigate("http://indigo.rafson.com.br/");
        }
    }
}
