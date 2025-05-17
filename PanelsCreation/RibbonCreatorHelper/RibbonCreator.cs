using Autodesk.Windows;
using PanelsCreation.UserInterface;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PanelsCreation.RibbonCreatorHelper
{
    internal class RibbonCreator
    {
        internal static BitmapImage getBitmap(string fileName)
        {
            BitmapImage bmp = new BitmapImage();
            // BitmapImage.UriSource must be in a BeginInit/EndInit block.             
            bmp.BeginInit();
            bmp.UriSource = new Uri(string.Format(
              "pack://application:,,,/{0};component/{1}",
              Assembly.GetExecutingAssembly().GetName().Name,
              fileName));
            bmp.EndInit();
            return bmp;
        }

        internal static RibbonTab GetRibbonTab(string title, string id)
        {
            RibbonTab ribbonTab = new RibbonTab();
            ribbonTab.Title = title;
            ribbonTab.Id = id;
            return ribbonTab;
        }
        internal static RibbonPanelSource GetRibbonPanelSource(string title, string id)
        {
            RibbonPanelSource panelSource = new RibbonPanelSource();
            panelSource.Title = title;
            panelSource.Id = id;
            return panelSource;
        }
        internal static RibbonPanel GetRibbonPanel(RibbonPanelSource panelSource)
        {
            RibbonPanel panel = new RibbonPanel();
            panel.Source = panelSource;
            return panel;
        }
        internal static RibbonButton GetRibbonButton(string id, string title,
            string command, string largeImagePath = null, string imagePath = null)
        {
            RibbonButton button = new RibbonButton
            {
                Text = title,
                Name = title,
                Id = id,
                ShowText = true,
                ShowImage = true,
                LargeImage = getBitmap(largeImagePath),
                Image = getBitmap(imagePath),
                Size = RibbonItemSize.Large,
                CommandHandler = new AdskCommandHandler(command)
            };

            return button;
        }
    }
    public class AdskCommandHandler : ICommand
    {
        private string commandName;

        public AdskCommandHandler(string command)
        {
            commandName = command;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            RibbonButton button = (RibbonButton)parameter;
            if (button == null) return;
            if (button.Id == "Draw Panel Design")
                PanelCreationCommand.DrawIncomingFullDesign();
            else if (button.Id == "Log in")
            {
                LoginUI login = new LoginUI();
                login.ShowDialog();
            }
            else if (button.Id == "Log out")
            {
                PanelCreationCommand.IsLoggedIn = false;
            }
        }
    }
}
