using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Utility
{
    public class SystemHelper
    {
        public static void CountForReview(int numStartToAsk)
        {
            IsolatedStorageSettings.ApplicationSettings["askforreview"] = false;

            int started = 0;
            if (IsolatedStorageSettings.ApplicationSettings.Contains("started"))
            {
                started = (int)IsolatedStorageSettings.ApplicationSettings["started"];
            }
            started++;
            IsolatedStorageSettings.ApplicationSettings["started"] = started;
            if (started == numStartToAsk)
            {
                IsolatedStorageSettings.ApplicationSettings["askforreview"] = true;
            }
        }

        public static void AskForReview(string appName)
        {
            var askforReview = (bool)IsolatedStorageSettings.ApplicationSettings["askforreview"];
            if (askforReview)
            {
                //make sure we only ask once! 
                IsolatedStorageSettings.ApplicationSettings["askforreview"] = false;
                var ok = MessageBox.Show(
                    string.Format("Cảm ơn bạn đã sử dụng {0}, Hãy đánh giá 5 sao cho ứng dụng và comment chức năng mà bạn muốn ở phiên bản kế tiếp.", appName), 
                    "Hãy đánh giá 5 sao!", MessageBoxButton.OKCancel);
                if (ok == MessageBoxResult.OK)
                {
                    var marketplaceReviewTask = new MarketplaceReviewTask();
                    marketplaceReviewTask.Show();
                }
            }
        }

        public static string GetAppID()
        {
            string appID = (from manifest in
                                System.Xml.Linq.XElement.Load("WMAppManifest.xml").Descendants("App")
                            select
                            manifest).SingleOrDefault().Attribute("ProductID").Value;
            return appID;
        }

        public static bool ShowExitMessageBox(string appName)
        {
            string caption = "Thoát";
            string message = string.Format("Bạn muốn thoát khỏi {0}?", appName);
            bool cancel = MessageBoxResult.Cancel == MessageBox.Show(message, caption,
            MessageBoxButton.OKCancel);
            return cancel;
        }

    }
}
