﻿using System.Collections;
using System.Collections.Generic;

namespace AKCore.DataModel
{
    public class AkWidgets
    {
        public const string TextImage = "TextImage";
        public const string Text = "Text";
        public const string Image = "Image";
        public const string Video = "Video";

        public static readonly IList Widgets = new List<string>()
        {
            TextImage,
            Text,
            Image,
            Video
        };
    }

    public class Widget
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public List<string> Videos { get; set; }

        public string GetHeader()
        {
            switch (Type)
            {
                case "Text":
                    return "Text-widget";
                case "Image":
                    return "Bild-widget";
                case "Video":
                    return "Video-widget";
            }
            return "Text-bild-widget";
        }
    }
}
