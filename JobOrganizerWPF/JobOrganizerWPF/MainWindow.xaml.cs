using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JobOrganizerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection connection = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mike\Documents\GitHub\TempJobOrganizer\JobOrganizerWPF\JobOrganizerWPF\DatabaseTest.mdf;Integrated Security=True");

        public MainWindow()
        {
            InitializeComponent();
            
            //GoogleCalendar.LoadCalendar();

            txtCalendarLink.Text = GoogleCalendar.myString;
            //myWebBrowser.Source = GoogleCalendar.myUri;

            FillCompanyList();
        }
        


        private void FillCompanyList()
        {           
            string companyQuery = "SELECT Company.Name FROM Company";
            SqlDataAdapter companyAdapt = new SqlDataAdapter(companyQuery, connection);
            
            DataTable companyTable = new DataTable();
            companyAdapt.Fill(companyTable);

            connection.Close();

            foreach (DataRow row in companyTable.Rows)
            {
                lstCompanies.Items.Add(row["Name"].ToString());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
            }
            catch (SqlException ex)
            {

                MessageBox.Show(ex.Message, Application.Current.MainWindow.GetType().Assembly.FullName, MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
