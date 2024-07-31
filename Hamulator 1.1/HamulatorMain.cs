using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hamulator_1._1
{
    public partial class HamulatorMain : Form
    {
        public int carTypeId = 1;

        public HamulatorMain()
        {
            InitializeComponent();
        }

        //Inicjalizacja potrzebnych paneli i pól możliwych do edytowania

        private void HamulatorMain_Load(object sender, EventArgs e)
        {
            pic1.Visible = true;
            pic2.Visible = false;
            pic3.Visible = false;

            cboxNawierzchnia.SelectedIndex = 0;
            cboxStanNawierzchni.SelectedIndex = 0;
            cboxStanKierowcy.SelectedIndex = 0;

            if (checkExpert.Checked == true)
            {
                panel3.Visible = true;
            }
            else
            {
                panel3.Visible = false;
            }

            if (checkPassagers.Checked == false)
            {
                numOsob.Enabled = false;
            }

            if (checkMass.Checked == false)
            {
                numMasa.Enabled = false;
            }
        }

        //Obsługa checkBox'a od wprowadzanie liczby pasażerów

        private void checkPassagers_CheckedChanged(object sender, EventArgs e)
        {
            if (checkPassagers.Checked == true)
            {
                numOsob.Enabled = true;
            }
            else if( checkPassagers.Checked == false) 
            {
                numOsob.Enabled = false;
            }
        }

        //Obsługa checkBox'a od wprowadzanie masy pojadu ręcznie

        private void checkMass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkMass.Checked == true)
            {
                numMasa.Enabled = true;
            }
            else if (checkMass.Checked == false)
            {
                numMasa.Enabled = false;
            }

        }

        //Przesyłanie wszystkich potrzebnych danych do form HamulatorWynik w zależności od wyboru użytkownika

        private void btnResult_Click(object sender, EventArgs e)
        {
            double frictionCoefficient = 0.7;

            if (cboxStanNawierzchni.Text == "sucha")
            {
                if (cboxNawierzchnia.Text == "asfalt")
                {
                    frictionCoefficient = 0.7;
                }
                else if (cboxNawierzchnia.Text == "kamienna")
                {
                    frictionCoefficient = 0.65;
                }
                else if (cboxNawierzchnia.Text == "gruntowa")
                {
                    frictionCoefficient = 0.6;
                }
            }
            else if(cboxStanNawierzchni.Text == "mokra")
            {
                if (cboxNawierzchnia.Text == "asfalt")
                {
                    frictionCoefficient = 0.6;
                }
                else if (cboxNawierzchnia.Text == "kamienna")
                {
                    frictionCoefficient = 0.5;
                }
                else if (cboxNawierzchnia.Text == "gruntowa")
                {
                    frictionCoefficient = 0.45;
                }
            }
            else if (cboxStanNawierzchni.Text == "ośnieżona")
            {
                if (cboxNawierzchnia.Text == "asfalt")
                {
                    frictionCoefficient = 0.5;
                }
                else if (cboxNawierzchnia.Text == "kamienna")
                {
                    frictionCoefficient = 0.4;
                }
                else if (cboxNawierzchnia.Text == "gruntowa")
                {
                    frictionCoefficient = 0.35;
                }
            }
            else if (cboxStanNawierzchni.Text == "oblodzona")
            {
                if (cboxNawierzchnia.Text == "asfalt")
                {
                    frictionCoefficient = 0.35;
                }
                else if (cboxNawierzchnia.Text == "kamienna")
                {
                    frictionCoefficient = 0.3;
                }
                else if (cboxNawierzchnia.Text == "gruntowa")
                {
                    frictionCoefficient = 0.25;
                }
            }
            

            HamulatorWynik HamulatorWynikForm = new HamulatorWynik();

            if (checkMass.Checked == true)
            {
                HamulatorWynikForm.carMass = Convert.ToInt32(numMasa.Value);
            }
              
            if (checkPassagers.Checked == true)
            {
                HamulatorWynikForm.carMass = Convert.ToInt32(numMasa.Value + numOsob.Value * 65);

            }

            HamulatorWynikForm.checkZaawansowane = checkExpert.Checked;
            HamulatorWynikForm.initialVelocityKmh = Convert.ToInt32(numV.Value);
            HamulatorWynikForm.slopePercentage = Convert.ToDouble(numSlope.Value);
            HamulatorWynikForm.carTypeId = Convert.ToInt32(carTypeId);
            HamulatorWynikForm.checkMasa = checkMass.Checked;

            if (checkExpert.Checked == true)
            {

                HamulatorWynikForm.frictionCoefficient = Convert.ToDouble(numFriction.Value);
                HamulatorWynikForm.reactionTime = Convert.ToDouble(numReactionTime.Value);
                HamulatorWynikForm.dragCoefficientAdjustment = Convert.ToDouble(numV.Value);
                HamulatorWynikForm.frontalArea = Convert.ToDouble(numV.Value);
                
            }
            else if (checkExpert.Checked == false)
            {
                
                HamulatorWynikForm.driverStatus = Convert.ToString(cboxStanKierowcy.Text);
                HamulatorWynikForm.frictionCoefficientBasic = Convert.ToString(cboxStanNawierzchni.Text + " - " + cboxNawierzchnia.Text);
                HamulatorWynikForm.frictionCoefficient = frictionCoefficient;
            }

            HamulatorWynikForm.Show();
        }

        //Obsługa zmiany i wyświetlania typu samochodu w przypadku kliknięcia "w lewo"

        private void picLeft_Click(object sender, EventArgs e)
        {
            if (pic1.Visible == true)
            {
                pic1.Visible = false;
                pic2.Visible = false;
                pic3.Visible = true;
                carTypeId = 3;
            }
            else if(pic2.Visible == true)
            {
                pic1.Visible = true;
                pic2.Visible = false;
                pic3.Visible = false;
                carTypeId = 1;
            }
            else if(pic3.Visible == true)
            {
                pic1.Visible = false;
                pic2.Visible = true;
                pic3.Visible = false;
                carTypeId = 2;
            }
        }

        //Obsługa zmiany i wyświetlania typu samochodu w przypadku kliknięcia "w prawo"

        private void picRight_Click(object sender, EventArgs e)
        {
            if (pic1.Visible == true)
            {
                pic1.Visible = false;
                pic2.Visible = true;
                pic3.Visible = false;
                carTypeId = 2;
            }
            else if (pic2.Visible == true)
            {
                pic1.Visible = false;
                pic2.Visible = false;
                pic3.Visible = true;
                carTypeId = 3;
            }
            else if (pic3.Visible == true)
            {
                pic1.Visible = true;
                pic2.Visible = false;
                pic3.Visible = false;
                carTypeId = 1;
            }
        }

        //Zmiana wyświetlanych paneli w zależności od checkBox'a trybu zaawansowanego

        private void checkExpert_CheckedChanged(object sender, EventArgs e)
        {
            if (checkExpert.Checked == true)
            {
                panel3.Visible = true;
            }
            else
            {
                panel3.Visible = false;
            }
        }

        //Informacje dla użytkownika, jak działa program po wciśnięciu ikonki informacji

        private void picInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("" +
                "Program Hamulator to narzędzie umożliwiające obliczenie drogi hamowania samochodu w zależności od różnych czynników, takich jak:\r\n-rodzaj nawierzchni, \r\n-stan nawierzchni, \r\n-prędkość początkowa, \r\n-kąt nachylenia drogi,\r\n-masa pojazdu,\r\n-stan kierowcy (czas reakcji)\r\n-Współczynnik tarcia\r\n-Opór powietrza w zależności od wybranego typu samochodu.\r\n\r\n" +
                "\r\nParametry wejściowe:\r\n\r\n-Rodzaj samochodu: Użytkownik może wybrać spośród trzech rodzajów samochodów: sportowego, rodzinnego i terenowego (Typy samochodów różnią się od siebie powierzchnią frontową, a co za tym idzie, innym oporem powietrza).\r\n-Stan nawierzchni: Umożliwia wybór stanu nawierzchni, takiego jak sucha, mokra, ośnieżona, czy oblodzona.\r\n-Typ nawierzchni: Pozwala na wybór rodzaju nawierzchni, takiej jak asfalt, kamienna czy gruntowa.\r\n-Stan kierowcy: Określa stan kierowcy, wpływający na czas reakcji (ostrożny, średni, starszy, odurzony/zmęczony).\r\n-Procent nachylenia drogi: Wprowadzenie liczby dodatniej, określane jest przez program jako droga \"pod górke\", a liczba ujemna jest uwzględniana jako droga \"z górki\".\r\n-Dodatkowe opcje: Umożliwiają uwzględnienie liczby pasażerów w pojeździe oraz dodanie dodatkowej masy.\r\n\r\n\r\n"
                + "Zaawansowane opcje:\r\n\r\nUmożliwiają ekspertom dostosowanie parametrów ręcznie, takich jak współczynnik tarcia, czy czas reakcji kierowcy.\r\n\r\n\r\n" 
                + "Wzory używane do obliczeń drogi hamowania:\r\n\r\nSiła Tarcia:\r\na_tarcie = współczynnikTarcia * efektywnaGrawitacja * dostosowanieTarcia\r\n\r\nSiła oporu aerodynamicznego:\r\nF_oporu = 0.5 * dostosowanieWspółczynnikaOporu * powierzchniaPrzodu * gęstośćPowietrza * V0^2;\r\n\r\nEfektywna siła oporu powietrza:\r\na_oporu = F_oporu / (masaSamochodu * efektywnaGrawitacja)\r\n\r\nDroga reakcji i percepcji kierowcy:\r\nD_reakcji_percepcji = V0 * czas_reakcji\r\n\r\nDroga hamowania:\r\nD_hamowania = V0^2 / (2 * (a_tarcie + a_oporu))\r\n\r\nCałkowita droga hamowania:\r\nD_calkowita = D_reakcji_percepcji + D_hamowania");
        }
    }
}
