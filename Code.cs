using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppPRATOFIORITO
{
    public partial class Form1 : Form
    {
        const int size = 25;
        const int percentualeBombe = 15;
        int dim = 0;
        int[,] campoGioco;
        int numBombe = 0;
        int conta = 0;

        public Form1()
        {
            InitializeComponent();            
        }

        private void btAvvia_Click(object sender, EventArgs e)
        {
            btAvvia.Visible = false;
            tb_dimensione.Visible = false;
            label1.Visible = false;

            do {

                dim = Convert.ToInt32(tb_dimensione.Text);

            } while (dim < 5 || dim > 30);
            CreaCampoGioco(dim);
            numBombe = NumeroBombe(dim);
            //Determino la posizione random delle bombe
            PosizionaBombe(dim, numBombe);
            CalcolaBombeAdiacenti(dim);

        }

        private void CreaCampoGioco(int dimensione)
        {
            for (int i = 0; i < dimensione; i++)
                for (int j = 0; j < dimensione; j++)
                { 
                    Button b = new Button();
                    b.Name = "bt_" + i + "_" + j;
                    b.Width = size;
                    b.Height = size;
                    b.Text = "";
                    b.Location = new Point(i * size , j * size);
                    b.Click += new System.EventHandler(cell_click);
                    b.MouseUp += new MouseEventHandler(Cel_RightClick);
                    //Aggiungo il button tra i controlli grafici del mio FORM
                    this.Controls.Add(b); //TUTTI I CONTROLLI AGGIUNTI DAL DESIGNER
                }
        }

        private int NumeroBombe(int dimensione)
        {
            return dimensione * dimensione * percentualeBombe / 100;
        }

        private void PosizionaBombe(int dimensione, int numBombe)
        {
            campoGioco = new int[dimensione, dimensione];

            Random r = new Random();
            int num = 0;
    
            while (num < numBombe)
            {
                int riga = r.Next(dimensione);
                int colonna = r.Next(dimensione);
                if (campoGioco[riga, colonna] != 10)
                {
                    num++;
                    campoGioco[riga, colonna] = 10;
                    //Controls lista di controlli grafici che usiamo comne fosse un array
                    //this.Controls["bt_" + riga + "_" + colonna].Text = "*";
                }
            }
        }

        private void CalcolaBombeAdiacenti(int dimensione)
        {
            for (int i = 0; i < dimensione; i++)
                for (int j= 0; j < dimensione; j++)
                {
                    //calcolo numero di bombe adiacenti solo se non ho una bomba nella casella
                    if(campoGioco[i,j] != 10 )
                    {
                        int tmpBombeVicine = 0;
                        //controllo la riga sopra
                        if(i != 0)
                        {
                            if (campoGioco[i - 1, j] == 10) tmpBombeVicine++;

                            if (j != 0)
                                if (campoGioco[i - 1, j - 1] == 10) tmpBombeVicine++;

                            if (j != dimensione - 1)
                                if (campoGioco[i - 1, j + 1] == 10) tmpBombeVicine++; 
                        }

                        //controllo la riga sotto
                        if (i != dimensione - 1)
                        {
                            if (campoGioco[i + 1, j] == 10) tmpBombeVicine++;

                            if (j != 0)
                                if (campoGioco[i + 1, j - 1] == 10) tmpBombeVicine++;

                            if (j != dimensione - 1)
                                if (campoGioco[i + 1, j + 1] == 10) tmpBombeVicine++;

                        }

                        //controllo la posizione di sinistra
                        if (j != 0)
                            if (campoGioco[i, j - 1] == 10) tmpBombeVicine++;

                        //controllo la posizione di destra
                        if (j != dimensione - 1) if (campoGioco[i, j + 1] == 10) tmpBombeVicine++;

                        campoGioco[i, j] = tmpBombeVicine;
                        //this.Controls["bt_" + i + "_" + j].Text = campoGioco[i, j].ToString();

                    }
                    
                }
        }

        private void cell_click(Object sender, EventArgs e)
        {
            //TO-DO: GESTIRE IL CLICK DELL'UTENTE SULLA CELLA
            Button myButton = (Button)sender;
            string[] riga1 = myButton.Name.Split('_');

            int riga = Convert.ToInt32(riga1[1]);
            int colonna = Convert.ToInt32(riga1[2]);

            if (campoGioco[riga, colonna] == 10)
            {
                MessageBox.Show("HAI PERSO!!!");

                for (int i = 0; i < dim; i++)
                {
                    for (int j = 0; j < dim; j++)
                    {
                        this.Controls["bt_" + i + "_" + j].Enabled = false;
                        if (campoGioco[i,j] == 10)
                        {
                            this.Controls["bt_" + i + "_" + j].Text = "*";
                        }
                    }
                }
            }

            else
            {
                myButton.Text = "" + campoGioco[riga, colonna];
                myButton.Enabled = false;
                RicorsioneZeri(riga, colonna);
                CondizioneVittoria(conta);
            }

        }

        private void Cel_RightClick(Object sender, MouseEventArgs e)
        {
            Button myButton = (Button)sender;

            if (e.Button == MouseButtons.Right)
            {
                if (myButton.Text == "?") myButton.Text = "";

                else  myButton.Text = "?";
            }
        }

        private void CondizioneVittoria(int conta)
        {
            int CelleDisp = (dim * dim) - numBombe;

            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    if (this.Controls["bt_" + i + "_" + j].Enabled == false && campoGioco[i, j] != 10)
                        conta++;
                }
            }

            if (conta == CelleDisp)
            {
                MessageBox.Show("HAI VINTO!!!");

                for (int i = 0; i < dim; i++)
                {
                    for (int j = 0; j < dim; j++)
                    {
                        this.Controls["bt_" + i + "_" + j].Enabled = false;

                        if (campoGioco[i, j] == 10)
                        {
                            this.Controls["bt_" + i + "_" + j].Text = "*";
                        }
                    }
                }
            }
        }

        private void RicorsioneZeri(int riga, int colonna)
        {

            if (campoGioco[riga, colonna] == 0)
            {
                if (riga != 0)
                {
                    if (campoGioco[riga - 1, colonna] != 10 && this.Controls["bt_" + (riga - 1) + "_" + colonna].Enabled == true)
                    {
                        this.Controls["bt_" + (riga - 1) + "_" + colonna].Enabled = false;
                        this.Controls["bt_" + (riga - 1) + "_" + colonna].Text = "" + campoGioco[riga - 1, colonna];

                        if (colonna - 1 >= 0)
                            RicorsioneZeri(riga, colonna - 1);
                        if (riga - 1 >= 0)
                            RicorsioneZeri(riga - 1, colonna);
                        if (riga - 1 >= 0 && colonna - 1 >= 0)
                            RicorsioneZeri(riga - 1, colonna - 1);
                        if (colonna + 1 < dim)
                            RicorsioneZeri(riga, colonna + 1);
                        if (riga + 1 < dim)
                            RicorsioneZeri(riga + 1, colonna);
                        if (riga + 1 < dim && colonna + 1 < dim)
                            RicorsioneZeri(riga + 1, colonna + 1);
                    }

                    if (colonna != 0)
                        if (campoGioco[riga - 1, colonna - 1] != 10 && this.Controls["bt_" + (riga - 1) + "_" + (colonna - 1)].Enabled == true)
                        {
                            this.Controls["bt_" + (riga - 1) + "_" + (colonna - 1)].Enabled = false;
                            this.Controls["bt_" + (riga - 1) + "_" + (colonna - 1)].Text = "" + campoGioco[riga - 1, colonna - 1];

                            if (colonna - 1 >= 0)
                                RicorsioneZeri(riga, colonna - 1);
                            if (riga - 1 >= 0)
                                RicorsioneZeri(riga - 1, colonna);
                            if (riga - 1 >= 0 && colonna - 1 >= 0)
                                RicorsioneZeri(riga - 1, colonna - 1);
                            if (colonna + 1 < dim)
                                RicorsioneZeri(riga, colonna + 1);
                            if (riga + 1 < dim)
                                RicorsioneZeri(riga + 1, colonna);
                            if (riga + 1 < dim && colonna + 1 < dim)
                                RicorsioneZeri(riga + 1, colonna + 1);
                        }

                    if (colonna != dim - 1)
                        if (campoGioco[riga - 1, colonna + 1] != 10 && this.Controls["bt_" + (riga - 1) + "_" + (colonna + 1)].Enabled == true)
                        {
                            this.Controls["bt_" + (riga - 1) + "_" + (colonna + 1)].Enabled = false;
                            this.Controls["bt_" + (riga - 1) + "_" + (colonna + 1)].Text = "" + campoGioco[riga - 1, colonna + 1];

                            if (colonna - 1 >= 0)
                                RicorsioneZeri(riga, colonna - 1);
                            if (riga - 1 >= 0)
                                RicorsioneZeri(riga - 1, colonna);
                            if (riga - 1 >= 0 && colonna - 1 >= 0)
                                RicorsioneZeri(riga - 1, colonna - 1);
                            if (colonna + 1 < dim)
                                RicorsioneZeri(riga, colonna + 1);
                            if (riga + 1 < dim)
                                RicorsioneZeri(riga + 1, colonna);
                            if (riga + 1 < dim && colonna + 1 < dim)
                                RicorsioneZeri(riga + 1, colonna + 1);
                        }
                }

                if (riga != dim - 1)
                {
                    if (campoGioco[riga + 1, colonna] != 10 && this.Controls["bt_" + (riga + 1) + "_" + colonna].Enabled == true)
                    {
                        this.Controls["bt_" + (riga + 1) + "_" + colonna].Enabled = false;
                        this.Controls["bt_" + (riga + 1) + "_" + colonna].Text = "" + campoGioco[riga + 1, colonna];

                        if (colonna - 1 >= 0)
                            RicorsioneZeri(riga, colonna - 1);
                        if (riga - 1 >= 0)
                            RicorsioneZeri(riga - 1, colonna);
                        if (riga - 1 >= 0 && colonna - 1 >= 0)
                            RicorsioneZeri(riga - 1, colonna - 1);
                        if (colonna + 1 < dim)
                            RicorsioneZeri(riga, colonna + 1);
                        if (riga + 1 < dim)
                            RicorsioneZeri(riga + 1, colonna);
                        if (riga + 1 < dim && colonna + 1 < dim)
                            RicorsioneZeri(riga + 1, colonna + 1);
                    }

                    if (colonna != 0)
                        if (campoGioco[riga + 1, colonna - 1] != 10 && this.Controls["bt_" + (riga + 1) + "_" + (colonna - 1)].Enabled == true)
                        {
                            this.Controls["bt_" + (riga + 1) + "_" + (colonna - 1)].Enabled = false;
                            this.Controls["bt_" + (riga + 1) + "_" + (colonna - 1)].Text = "" + campoGioco[riga + 1, colonna - 1];

                            if (colonna - 1 >= 0)
                                RicorsioneZeri(riga, colonna - 1);
                            if (riga - 1 >= 0)
                                RicorsioneZeri(riga - 1, colonna);
                            if (riga - 1 >= 0 && colonna - 1 >= 0)
                                RicorsioneZeri(riga - 1, colonna - 1);
                            if (colonna + 1 < dim)
                                RicorsioneZeri(riga, colonna + 1);
                            if (riga + 1 < dim)
                                RicorsioneZeri(riga + 1, colonna);
                            if (riga + 1 < dim && colonna + 1 < dim)
                                RicorsioneZeri(riga + 1, colonna + 1);
                        }

                    if (colonna != dim - 1)
                        if (campoGioco[riga + 1, colonna + 1] != 10 && this.Controls["bt_" + (riga + 1) + "_" + (colonna + 1)].Enabled == true)
                        {
                            this.Controls["bt_" + (riga + 1) + "_" + (colonna + 1)].Enabled = false;
                            this.Controls["bt_" + (riga + 1) + "_" + (colonna + 1)].Text = "" + campoGioco[riga + 1, colonna + 1];

                            if (colonna - 1 >= 0)
                                RicorsioneZeri(riga, colonna - 1);
                            if (riga - 1 >= 0)
                                RicorsioneZeri(riga - 1, colonna);
                            if (riga - 1 >= 0 && colonna - 1 >= 0)
                                RicorsioneZeri(riga - 1, colonna - 1);
                            if (colonna + 1 < dim)
                                RicorsioneZeri(riga, colonna + 1);
                            if (riga + 1 < dim)
                                RicorsioneZeri(riga + 1, colonna);
                            if (riga + 1 < dim && colonna + 1 < dim)
                                RicorsioneZeri(riga + 1, colonna + 1);
                        }

                    //controllo la posizione di sinistra
                    if (colonna != 0)
                        if (campoGioco[riga, colonna - 1] != 10 && this.Controls["bt_" + riga + "_" + (colonna - 1)].Enabled == true)
                        {
                            this.Controls["bt_" + riga + "_" + (colonna - 1)].Enabled = false;
                            this.Controls["bt_" + riga + "_" + (colonna - 1)].Text = "" + campoGioco[riga, colonna - 1];

                            if (colonna - 1 >= 0)
                                RicorsioneZeri(riga, colonna - 1);
                            if (riga - 1 >= 0)
                                RicorsioneZeri(riga - 1, colonna);
                            if (riga - 1 >= 0 && colonna - 1 >= 0)
                                RicorsioneZeri(riga - 1, colonna - 1);
                            if (colonna + 1 < dim)
                                RicorsioneZeri(riga, colonna + 1);
                            if (riga + 1 < dim)
                                RicorsioneZeri(riga + 1, colonna);
                            if (riga + 1 < dim && colonna + 1 < dim)
                                RicorsioneZeri(riga + 1, colonna + 1);
                        }
                    //controllo la posizione di destra
                    if (colonna != dim - 1)
                        if (campoGioco[riga, colonna + 1] != 10 && this.Controls["bt_" + riga + "_" + (colonna + 1)].Enabled == true)
                        {
                            this.Controls["bt_" + riga + "_" + (colonna + 1)].Enabled = false;
                            this.Controls["bt_" + riga + "_" + (colonna + 1)].Text = "" + campoGioco[riga, colonna + 1];

                            if (colonna - 1 >= 0)
                                RicorsioneZeri(riga, colonna - 1);
                            if (riga - 1 >= 0)
                                RicorsioneZeri(riga - 1, colonna);
                            if (riga - 1 >= 0 && colonna - 1 >= 0)
                                RicorsioneZeri(riga - 1, colonna - 1);
                            if (colonna + 1 < dim)
                                RicorsioneZeri(riga, colonna + 1);
                            if (riga + 1 < dim)
                                RicorsioneZeri(riga + 1, colonna);
                            if (riga + 1 < dim && colonna + 1 < dim)
                                RicorsioneZeri(riga + 1, colonna + 1);
                        }

                }
            }
        }
    }
}
