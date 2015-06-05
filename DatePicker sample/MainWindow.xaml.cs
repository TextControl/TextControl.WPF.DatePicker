using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace DatePicker_sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        // create the Canvas for hosting the DatePick control
        Canvas dateHolder = null;
        // the ApplicationField which will be used for validation
        TXTextControl.ApplicationField selectedField = null;
        private void textControl_TextFieldClicked(object sender, TXTextControl.TextFieldEventArgs e)
        {
            // get the field that has been clicked into
            selectedField = (TXTextControl.ApplicationField)e.TextField;
            
            // if the field is a DATE field a DatePick control will be used
            if (selectedField.TypeName == "DATE")
            {
                // get the position where the mouse click occured so we can use it for the Canvas
                Point mousePos = Mouse.GetPosition(textControl);

                // creating and setting the DatePicker up
                DatePicker date = new DatePicker();
                date.Name = "date1";
                date.Background = Brushes.LightBlue;
                date.DisplayDate = DateTime.Today;
                date.IsTodayHighlighted = true;
                date.SelectedDateChanged += date_SelectedDateChanged;
                // creating and setting the Canvas up, which will hold the DatePicker
                dateHolder = new Canvas();
                dateHolder.Background = Brushes.AliceBlue;
                dateHolder.Height = 1;
                dateHolder.Width = 1;
                dateHolder.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                dateHolder.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                // the position needs to be adjusted slightly so the DatePicker will not be displayed
                // directly on top of the date field
                dateHolder.Margin = new Thickness(mousePos.X-40, mousePos.Y-40, 0, 0);
                date.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                date.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                
                // adding the DatePicker to the Canvas
                dateHolder.Children.Add(date);
                // adding the Canvas to the Grid
                Grid1.Children.Add(dateHolder);
                // let's show the Canvas with the DatePicker to the user
                dateHolder.BringIntoView();
            }
        }

        void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // validation if a field has been selected previously
            if (selectedField != null)
            {
                // DATE FIELD VALIDATION IS MISSING HERE IF MORE THAN ONE FIELD TYPE IS TO BE HANDLED!
                /// there are a couple of other fields that can be handled in the same way, see
                //  http://www.textcontrol.com/en_US/support/documentation/dotnet/n_txdotnet.ref.txtextcontrol.documentserver.fields.htm
                TXTextControl.DocumentServer.Fields.DateField date = new TXTextControl.DocumentServer.Fields.DateField(selectedField);
                // set the selected date
                date.Date = (DateTime)e.AddedItems[0];
            }
            // remove the Canvas, job done
            Grid1.Children.Remove(dateHolder);
        }

        private void textControl_Loaded(object sender, RoutedEventArgs e)
        {
            textControl.Focus();

            textControl.Selection.Text = "Hello World. Today's date: ";

            TXTextControl.DocumentServer.Fields.DateField date = new TXTextControl.DocumentServer.Fields.DateField();
            // we only want to display the date
            date.Format = "d";
            date.ApplicationField.ShowActivated = true;
            date.ApplicationField.DoubledInputPosition = true;
            // set the current date to the field's content
            date.Date = DateTime.Now;
            textControl.ApplicationFields.Add(date.ApplicationField); 
        }

        private void textControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // if a Canvas exists, but a click happens in the TextControl the DatePicker should probably be closed
            if (dateHolder != null)
            {
                // remove the Canvas
                Grid1.Children.Remove(dateHolder);
            }
        }
    }
}
