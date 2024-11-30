
namespace ContactsApp.Properties
{
    using System;
    using System.Drawing;
    using System.Resources;

    internal static class Resources
    {
        private static ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;

        public static ResourceManager ResourceManager
        {
            get
            {
                if (resourceMan == null)
                {
                    resourceMan = new ResourceManager("ContactsApp.Resources", typeof(Resources).Assembly);
                }
                return resourceMan;
            }
        }

        public static global::System.Globalization.CultureInfo Culture
        {
            get { return resourceCulture; }
            set { resourceCulture = value; }
        }

        public static Bitmap add_icon
        {
            get
            {
                return (Bitmap)ResourceManager.GetObject("add_icon", resourceCulture);
            }
        }

        public static Bitmap edit_icon
        {
            get
            {
                return (Bitmap)ResourceManager.GetObject("edit_icon", resourceCulture);
            }
        }

        public static Bitmap remove_icon
        {
            get
            {
                return (Bitmap)ResourceManager.GetObject("remove_icon", resourceCulture);
            }
        }
    }
}
