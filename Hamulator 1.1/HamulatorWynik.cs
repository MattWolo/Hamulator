using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Hamulator_1._1
{
    public partial class HamulatorWynik : Form
    {

        // Właściwości reprezentujące parametry wejściowe symulacji.
        public int initialVelocityKmh { get; set; }
        public double frictionCoefficient { get; set; }
        public string driverStatus { get; set; }
        public double slopePercentage { get; set; }
        public double dragCoefficientAdjustment { get; set; }
        public double frontalArea { get; set; }
        public int carMass { get; set; }
        public int carTypeId { get; set; }
        public string frictionCoefficientBasic { get; set; }
        public bool checkZaawansowane { get; set; }
        public bool checkMasa { get; set; }
        public double reactionTime { get; set; }


        // Stałe używane w obliczeniach fizycznych.
        const double Gravity = 9.8;
        const double AirDensity = 1.225;

        public HamulatorWynik()
        {
            InitializeComponent();
        }

        private void HamulatorWynik_Load(object sender, EventArgs e)
        {
            // Sprawdzenie warunków zaawansowanych i ustawienie widoczności odpowiednich paneli.

            if (checkZaawansowane == false)
            {
                panel5.Visible = false;

                // Ustawienie czasu reakcji kierowcy na podstawie wybranego statusu.

                if (driverStatus == "ostrożny - 1 s")
                {
                    reactionTime = 1;
                }
                else if (driverStatus == "średnia kierowców - 1.5 s")
                {
                    reactionTime = 1.5;
                }
                else if (driverStatus == "starszy kierowca - 2 s")
                {
                    reactionTime = 2;
                }
                else if (driverStatus == "odurzony/zmęczony - 2.5 s")
                {
                    reactionTime = 2.5;
                }
                lbNawierzchnia.Text = frictionCoefficientBasic;
            }
            else if(checkZaawansowane == true) 
            {
                lbFriction.Text = Convert.ToString(frictionCoefficient) + " μ";
            }

            // Konwersja identyfikatora typu samochodu na tekst.

            string carType = "xyz";
            switch (carTypeId)
            {
                default:
                    break;
                case 1:
                    carType = "sportowe";
                    break;
                case 2:
                    carType = "rodzinne";
                    break;
                case 3:
                    carType = "terenowe";
                    break;
            }

            // Obliczenie wyniku za pomocą funkcji

            double initialVelocityMs = ConvertKmhToMs(initialVelocityKmh);
            double wynik = CalculateBrakingDistance(frictionCoefficient, initialVelocityMs, reactionTime, slopePercentage, carTypeId, checkMasa);

            // Ustawienie tekstów w kontrolkach formularza.

            lbReactionTime.Text = reactionTime + " s";
            lbTyp.Text = carType;
            lbV.Text = Convert.ToString(initialVelocityKmh) + " km/h";
            lbMasa.Text = Convert.ToString(carMass) + " kg";
            lbResult.Text = Math.Round(wynik, 4) + " m";

            // Wykres predkość-czas.

            Series seriesPredkoscCzas = new Series("PredkoscCzas");
            seriesPredkoscCzas.ChartType = SeriesChartType.Line;
            chart1.Series.Add(seriesPredkoscCzas);

            // Dodanie punktów do wykresu.

            for (double czas = 0; czas <= reactionTime + wynik; czas += 0.1)
            {
                double predkosc = CalculateSpeed(initialVelocityMs, czas);
                seriesPredkoscCzas.Points.AddXY(czas, ConvertMsToKmh(predkosc));
            }

            // Konfiguracja osi X wykresu.

            chart1.ChartAreas[0].AxisX.Interval = 1; 
            chart1.ChartAreas[0].AxisX.Minimum = 0;

            // Dostosowanie zakresu osi X w zależności od wyniku symulacji.

            if (wynik > 100)
            {
                chart1.ChartAreas[0].AxisX.Maximum = 20;
                chart1.ChartAreas[0].AxisX.Interval = 5;
            }
            else if (wynik < 10)
            {
                chart1.ChartAreas[0].AxisX.Maximum = 2;
                chart1.ChartAreas[0].AxisX.Interval = 0.5;
            }
            else if(wynik < 20)
            {
                chart1.ChartAreas[0].AxisX.Maximum = 5;
            }
            else if (wynik < 100)
            {
                chart1.ChartAreas[0].AxisX.Maximum = 10;
                chart1.ChartAreas[0].AxisX.Interval = 1;
            }

            // Ustawienie tytułów osi wykresu.

            chart1.ChartAreas[0].AxisX.Title = "t [s]";

            chart1.ChartAreas[0].AxisY.Title = "v [km/h]";

        }

        // Metoda obliczająca odległość hamowania.

        public double CalculateBrakingDistance( double frictionCoefficient, double initialVelocity, double reactionTime, double slopePercentage, int carTypeId, bool checkMasa)
        {
            // Inicjalizacja parametrów dla obliczeń fizycznych.

            double effectiveGravity = Gravity * (1 + slopePercentage / 100);
            double frictionAdjustment = 1.0;
            double dragCoefficientAdjustment = 1.0;
            double frontalArea = 2.2;


            // Wybór parametrów dla konkretnego typu samochodu.

            switch (carTypeId)
                {
                    case 1:
                        dragCoefficientAdjustment = 0.38;
                        frontalArea = 1.7;

                        if (checkMasa == false)
                        {
                            carMass = 975;
                        }

                        break;
                    case 2:
                        dragCoefficientAdjustment = 0.32;
                        frontalArea = 2.3;
                        if (checkMasa == false)
                        {
                            carMass = 1500;
                        }
                        break;
                    case 3:
                        dragCoefficientAdjustment = 0.45;
                        frontalArea = 2.8;
                        if (checkMasa == false)
                        {
                            carMass = 2800;
                        }
                        break;
                    default:
                        break;
                }

            // Obliczenia fizyczne odległości hamowania na podstawie wszystkich danych wprowadzonych/wybranych.

            double decelerationFriction = frictionCoefficient * effectiveGravity * frictionAdjustment;

            double dragForce = 0.5 * dragCoefficientAdjustment * frontalArea * AirDensity * initialVelocity * initialVelocity;

            double decelerationDrag = dragForce / (carMass * effectiveGravity);

            double perceptionReactionDistance = initialVelocity * reactionTime;

            double brakingDistance = (initialVelocity * initialVelocity) / (2 * (decelerationFriction + decelerationDrag));

            double totalBrakingDistance = perceptionReactionDistance + brakingDistance;

            return totalBrakingDistance;
        }

        // Metoda obliczająca prędkość w danym momencie czasu, dane potrzebne do stworzenia wykresu.

        public double CalculateSpeed(double initialVelocity, double time)
        {

            // Inicjalizacja parametrów dla obliczeń fizycznych.
            double effectiveGravity = Gravity * (1 + slopePercentage / 100);
            double frictionAdjustment = 1.0;
            double dragCoefficientAdjustment = 1.0;
            double frontalArea = 2.2;

            // Wybór parametrów dla konkretnego typu samochodu.

            switch (carTypeId)
            {
                case 1:
                    dragCoefficientAdjustment = 0.38;
                    frontalArea = 1.7;

                    if (checkMasa == false)
                    {
                        carMass = 975;
                    }

                    break;
                case 2:
                    dragCoefficientAdjustment = 0.32;
                    frontalArea = 2.3;
                    if (checkMasa == false)
                    {
                        carMass = 1500;
                    }
                    break;
                case 3:
                    dragCoefficientAdjustment = 0.45;
                    frontalArea = 2.8;
                    if (checkMasa == false)
                    {
                        carMass = 2800;
                    }
                    break;
                default:
                    break;
            }

            // Obliczenia fizyczne prędkości.

            double decelerationFriction = frictionCoefficient * effectiveGravity * frictionAdjustment;
            double dragForce = 0.5 * dragCoefficientAdjustment * frontalArea * AirDensity * initialVelocity * initialVelocity;
            double decelerationDrag = dragForce / (carMass * effectiveGravity);

            double speed = initialVelocity - (decelerationFriction + decelerationDrag) * time;

            return Math.Max(0, speed);
        }

        // Metoda konwertująca prędkość z m/s na km/h.

        public static double ConvertMsToKmh(double velocityMs)
        {
            return velocityMs * 3600 / 1000;
        }

        // Metoda konwertująca prędkość z km/h na m/s.

        public static double ConvertKmhToMs(double velocityKmh)
        {
            return velocityKmh * 1000 / 3600;
        }
    }
}
