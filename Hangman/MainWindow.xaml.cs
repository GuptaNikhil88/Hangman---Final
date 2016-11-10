using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Data.OleDb;
using System.Collections.ObjectModel;
using System.IO;



namespace Hangman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int counter = 0;
        public string final = " ";
        public int count = 0;
        public string word;
        public int score = 0;
        public int flag = 0;
        public string[] index;
        public string[,] fin;
        public int levelselector;
        public int num = 0;
        public int hintcount = 1;
        public MainWindow()
        {
            InitializeComponent();
            //System.Media.SoundPlayer sp = new System.Media.SoundPlayer();
            //sp.Stream = Hangman.Properties.Resources.hangman;
            //sp.PlayLooping();
            gridkeys.Visibility = Visibility.Collapsed;
           // label.Visibility = Visibility.Collapsed;
            textblock.Visibility = Visibility.Collapsed;
            strt.Visibility = Visibility.Collapsed;
           DispatcherTimer dispatch = new DispatcherTimer();
           dispatch.Tick += new EventHandler(dispatch_tick);
           dispatch.Interval = new TimeSpan(0, 0, 1);
           dispatch.Start();
           moveimage();
            levels.Visibility = Visibility.Collapsed;
            hangman.Visibility = Visibility.Collapsed;
            hang.Visibility = Visibility.Collapsed;
            Extra.Visibility = Visibility.Collapsed;

        }

        private void dispatch_tick(object sender, EventArgs e)
        {
            counter++;
            if(counter%12==0)
            {
                moveimage();
            }
        }

        private void moveimage()
        {

            try { 
            TranslateTransform ttx = new TranslateTransform();
            TranslateTransform tty = new TranslateTransform();

            DoubleAnimationUsingKeyFrames dax = new DoubleAnimationUsingKeyFrames();
            dax.KeyFrames.Add(new LinearDoubleKeyFrame(960, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(12))));

            
            TransformGroup tg = new TransformGroup();
            tg.Children.Add(ttx);
            tg.Children.Add(tty);
            
            image.RenderTransform = tg;
            
            ttx.BeginAnimation(TranslateTransform.XProperty, dax);
            }
            catch(Exception e)
            {
                string errorMsg = "An application error occurred. Please contact the adminstrator " +
        "with the following information:\n\n";
                errorMsg = errorMsg + e.Message + "\n\nStack Trace:\n" + e.StackTrace;
                MessageBox.Show(errorMsg);
            }


        }

        public void loadwords()
        {
            int i = 0;
            try
            {
                //string[] readwords = File.ReadAllLines("Words.csv");
                string[] readwords = Hangman.Properties.Resources.Words.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string w in readwords)
                {
                    string[] lines = w.Split(',');
                    if (lines[0] == Convert.ToString(levelselector))
                    {
                        count++;
                    }
                }

                fin = new string[count, 4];

                foreach (string w in readwords)
                {
                    string[] lines = w.Split(',');
                    if (lines[0] == Convert.ToString(levelselector))
                    {
                        fin[i, 0] = lines[1];
                        fin[i, 1] = lines[2];
                        fin[i, 2] = lines[3];
                        fin[i, 3] = lines[4];

                        i++;
                    }
                    else
                        continue;
                }
            }
            catch (FileNotFoundException exp)
            {

                string errorMsg = "An application error occurred. Please contact the adminstrator " +
       "with the following information:\n\n";
                errorMsg = errorMsg + exp.Message + "\n\nStack Trace:\n" + exp.StackTrace;
                MessageBox.Show(errorMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        private void button1_Click(object sender, RoutedEventArgs e)
        {
            levels.Visibility = Visibility.Visible;
            gridkeys.IsEnabled =! IsEnabled;
            endpoint.Visibility = Visibility.Collapsed;
            hang.Visibility = Visibility.Collapsed;
            gridkeys.Visibility = Visibility.Collapsed;
            hangman.Visibility = Visibility.Collapsed;
            label.Visibility = Visibility.Collapsed;
            strt.Visibility = Visibility.Collapsed;
            textblock.Text = " ";
            score = 0;
            label1.Content = " "; 

        }

        

        private void strt_Click(object sender, RoutedEventArgs e)
        {
            int length = 0;
            label.Visibility = Visibility.Visible;
            Random random = new Random();
            try { 
            int i = fin.GetLength(0);
            flag = random.Next(0,i);

            while(fin[flag,3]==Convert.ToString(1))
            {
                flag = random.Next(0, i - 1);
            }
            length = fin[flag,0].Length;
            word = fin[flag,0].ToUpper();

            textblock.Text = value(length);
            gridkeys.IsEnabled = IsEnabled;
            MessageBox.Show(fin[flag,1]);
            strt.Visibility = Visibility.Collapsed;
            count = 0;
            }
            catch(Exception exp) {

                string errorMsg = "An application error occurred. Please contact the adminstrator " +
       "with the following information:\n\n";
                errorMsg = errorMsg + exp.Message + "\n\nStack Trace:\n" + exp.StackTrace;
                MessageBox.Show(errorMsg);
            }
        }

        private string value(int t)
        {
            textblock.Text = " ";
            string text = "_";
                             
            for (int i=0;i< (2*t - 1); i++)
            {
                if (i % 2 == 0)
                {
                    if (i == 0 )
                        text = "_";
                    
                    if (i!= 0 )
                        text = text + "_";
                    
                }
                else
                    text = text + " " + " ";
                
            }
            
            return text;
        }

        private void wordfill(char alpha, int key)
        {
            int l = word.Length;
            
            char[] letter = word.ToCharArray();
            char[] text = textblock.Text.ToCharArray(); 
            for (int i = 0; i < l; i++)
            {
                if (alpha == letter[i])
                {
                    //flag = i + 1;
                    //textblock.Text = value(l);
                    if (i == 0)
                    {
                        text[i] = alpha;
                        //final = Convert.ToString(alpha);
                    }
                    else { 
                        text[3 * i] = alpha;
                    //final = final + Convert.ToString(alpha);
                    }
                }
                else
                {
                //    flag = 0;
                    continue;
                }
            }
            string fw = new string(text);
            textblock.Text = fw;
            int len = text.Length;
            for(int z = 0; z< len;z++)
            {
                if (z == 0)
                    final = Convert.ToString(text[z]);
                else {
                    if (text[z] != ' ' || text[z] != '_')
                    {
                        final = final + text[z];
                    }
                    else
                        continue;
                }
            }
                       
            final = final.Replace(" ", "");

        }

        
        

        private void a_Click(object sender, RoutedEventArgs e)
        {
            Button pressed = sender as Button;
            //SystemSounds.Asterisk.Play();
            string name = Convert.ToString(pressed.Content);
            if (name == "Quit")
                Application.Current.Shutdown();
            

            bool has = word.Contains(name);
            
            if (has)
            {

                wordfill(Convert.ToChar(name), 1);
                pressed.Visibility = Visibility.Collapsed;
                if (final == word)
                {
                    score++;
                    label1.Content = Convert.ToString(score);  
                    MessageBox.Show("Yay, Good Job Champ!!", "Congratulations");
                    gridkeys.IsEnabled = !IsEnabled;
                    strt.Visibility = Visibility.Visible;
                    textblock.Text = " ";
                    int sum = 0;
                    for (int y = 0; y <= fin.GetLength(0)-1; y++)
                    {
                        sum = sum + Convert.ToInt32(fin[y, 3]);
                    }
                    if (sum == fin.GetLength(0))
                    {
                        MessageBox.Show("You have completed the Level, Congratulations", "Level Up");
                    }
                    else
                    { 
                    strt.Content = "Next Word";
                    }
                    hintcount = 1;
                    resetcontrols();
                    fin[flag, 3] = Convert.ToString(1);
                    
                    
                }
            }
            else
            { 
                count++;
                SystemSounds.Asterisk.Play();
               // label.Content = Convert.ToString(count);
            pressed.Visibility = Visibility.Collapsed;
            }
            BitmapImage bi = new BitmapImage();
            //IntPtr bi = new IntPtr();
            bi.BeginInit();
            if (count == 1)
                hangman.Visibility = Visibility.Visible;
            if (count == 2) {
                //hangman.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Head.png");
                //hangman.Source = new BitmapImage(new Uri("Head.png"));
                 bi.UriSource = new Uri("Head.png", UriKind.Relative);
                bi.EndInit();
                 hangman.Source = bi;
                //hangman.Source = new BitmapImage(Hangman.Properties.Resources.Head);
                //bi = global::Hangman.Properties.Resources.Head.GetHbitmap();
                //hangman.ima;

            }
            if (count == 3)
            {
                //hangman.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Body.png");
                bi.UriSource = new Uri("Body.png", UriKind.Relative);
                bi.EndInit();
                hangman.Source = bi;
            }
            if (count == 4)
            {
                //hangman.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Hands.png");
                bi.UriSource = new Uri("Hands.png", UriKind.Relative);
                bi.EndInit();
                hangman.Source = bi;
            }
            if (count == 5)
            {
                //hangman.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Full.png");
                bi.UriSource = new Uri("Full.png", UriKind.Relative);
                bi.EndInit();
                hangman.Source = bi;
                
                MessageBox.Show("Game Over", "Aww!");
                hangman.Visibility = Visibility.Collapsed;
                hang.Visibility = Visibility.Visible;
                gridkeys.Visibility = Visibility.Collapsed;
                strt.Visibility = Visibility.Collapsed;
                endpoint.Visibility = Visibility.Visible;
                textblock.Visibility = Visibility.Collapsed;
                //System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                //Application.Current.Shutdown();
                 
            }

        }
        private void resetcontrols()
        {
            count = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri("Rope1.png", UriKind.Relative);
            bi.EndInit();
            hangman.Source = bi;
            hangman.Visibility = Visibility.Collapsed;
            foreach (Button button in gridkeys.Children)
            {
                button.Visibility = Visibility.Visible;
            }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            //mp.Open(new Uri("Super Mario.wav", UriKind.Relative));
            //mp.Play();
        }

        private void getwords()
        {
            
        }

        private void level1_Click(object sender, RoutedEventArgs e)
        {
            Button pressed = sender as Button;
            textblock.Text = " ";
            string name = Convert.ToString(pressed.Content);
            if (name == "Level 1")
                levelselector = 1;
            else if (name == "Level 2")
                levelselector = 2;
            else
                levelselector = 3;
            Extra.Visibility = Visibility.Visible;
            gridkeys.Visibility = Visibility.Visible;
            //label.Visibility = Visibility.Visible;
            textblock.Visibility = Visibility.Visible;
            strt.Visibility = Visibility.Visible;
            button1.Visibility = Visibility.Collapsed;
            levels.Visibility = Visibility.Collapsed;
            resetcontrols();
            strt.Content = "Start";
            loadwords();
        }

        private void button_Copy25_Click(object sender, RoutedEventArgs e)
        {
            
            if(hintcount==2)
            {
                MessageBox.Show("You are out of hints");
            }
            else
            { 
            MessageBox.Show(fin[flag, hintcount+1]);
            hintcount++;
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
    
    

