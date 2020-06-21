using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace JARVIS.Telas
{
    public partial class TelaBingo : Form
    {
        public SpeechSynthesizer resposta = new SpeechSynthesizer();
        private List<int> listNumbers = new List<int>();

        public TelaBingo()
        {
            InitializeComponent();

            resposta.SpeakAsync("Okay, vamos começar");

            timer1.Start();
        }

        private int GetNumberRandom()
        {
            Random numberRandom = new Random();

            int number = numberRandom.Next(1, 4);

            if (!listNumbers.Contains(number))
            {
                listNumbers.Add(number);
                lstPedrasBingo.Items.Add(Convert.ToString(number));

                string numberSorted = $"O número sorteado foi o {number}";
                resposta.SpeakAsync(numberSorted);
            }

            return number;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GetNumberRandom();         

            if (listNumbers.Count.Equals(3))
            {
                resposta.SpeakAsync("Os números acabaram, verifique se você ou quem está jogando com você não comeu bronha.");

                timer1.Stop();
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
