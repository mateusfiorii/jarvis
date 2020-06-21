using JARVIS.Telas;
using Microsoft.Speech.Recognition;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace JARVIS
{
    public partial class Form1 : Form
    {
        public static CultureInfo culture = new CultureInfo("pt-BR");
        public static SpeechRecognitionEngine reconhecedor = null;
        public SpeechSynthesizer resposta = new SpeechSynthesizer();

        public string[] listaPalavras = {
            "oi",
            "quem é você",
            "como você está",
            "me fale algo da mamãe",
            "Quem é o Cesar",
            "Me fala da Jéssica",
            "Conte-me da Cris louca",
            "Que dia é hoje",
            "Vamos jogar bingo",
            "Jarvis, pode finalizar o sistema"
        };

        public string[] comandosProgramas =
        {
            "abrir calculadora",
            "abrir sql server"
        };

        public Form1()
        {
            InitializeComponent();
            Gramatica();
        }

        public void Gramatica()
        {
            try
            {
                reconhecedor = new SpeechRecognitionEngine(culture);
            }
            catch (Exception ex)
            {
                throw new Exception("ERRO ao integrar a lingua escolhida: " + ex.Message);
            }

            var gramatica = new Choices();
            gramatica.Add(listaPalavras);
            gramatica.Add(comandosProgramas);

            var gramaticaBuilder = new GrammarBuilder();
            gramaticaBuilder.Append(gramatica);

            try
            {
                var grammar = new Grammar(gramaticaBuilder);

                try
                {
                    reconhecedor.RequestRecognizerUpdate();
                    reconhecedor.LoadGrammarAsync(grammar);
                    reconhecedor.SpeechRecognized += Reconhecedor_SpeechRecognized;
                    reconhecedor.SetInputToDefaultAudioDevice();
                    resposta.SetOutputToDefaultAudioDevice();
                    reconhecedor.RecognizeAsync(RecognizeMode.Multiple);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERRO ao criar reconhecedor: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRO ao criar a gramatica: " + ex.Message);
            }
        }

        void Reconhecedor_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string frase = e.Result.Text;



            switch (frase)
            {
                case "Que dia é hoje":
                    string[] data = DateTime.Now.Date.ToString().Split(' ');

                    resposta.SpeakAsync(data[0]);
                    break;
                case "Vamos jogar bingo":
                    resposta.Speak("Okay, irei redirecioná-lo para a tela do jogo");

                    TelaBingo telaBingo = new TelaBingo();
                    telaBingo.ShowDialog();
                    break;
                case "Jarvis, pode finalizar o sistema":
                    resposta.SpeakAsync("Okay, estou indo dormir");
                    Application.Exit();
                    break;
                case "abrir calculadora":
                    resposta.SpeakAsync("Estou abrindo a calculadora");

                    Process.Start("calc.exe");
                    break;
                case "abrir sql server":
                    resposta.SpeakAsync("Estou abrindo o SQL Server");
                    Process.Start(@"C:\Program Files (x86)\Microsoft SQL Server Management Studio 18\Common7\IDE\Ssms.exe");
                    break;
                default:
                    resposta.SpeakAsync("Não entendi o que falou");
                    break;
            }
        }
    }
}
