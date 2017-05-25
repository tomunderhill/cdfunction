using System;
using System.Collections.Generic;

namespace FunctionsLibraryProject
{
    [Serializable]
    public class OcrResult
    {
        public string language { get; set; }
        public string textAngle { get; set; }
        public string orientation { get; set; }
        public IEnumerable<Region> regions { get; set; }
    }

    [Serializable]
    public class Region
    {
        public string boundingBox { get; set; }
        public IEnumerable<Line> lines { get; set; }
    }

    [Serializable]
    public class Line
    {
        public string boundingBox { get; set; }
        public IEnumerable<Word> words { get; set; }
    }

    [Serializable]
    public class Word
    {
        public string boundingBox { get; set; }
        public string text { get; set; }
    }

    [Serializable]
    public class GoogleBooksResult
    {
        public string kind { get; set; }
        public IEnumerable<GoogleBookItem> items { get; set; }
    }

    [Serializable]
    public class GoogleBookItem
    {
        public string kind { get; set; }
        public string id { get; set; }
        public string etag { get; set; }
        public string selfLink { get; set; }
        public VolumeInfo volumeInfo { get; set; }
    }

    [Serializable]
    public class VolumeInfo
    {
        public string title { get; set; }
        public string publisher { get; set; }
        public string publishedDate { get; set; }
        public IEnumerable<string> authors { get; set; }
        public string description { get; set; }
        public IEnumerable<IndustryIdentifier> industryIdentifiers { get; set; }
        public ImageLink imageLinks { get; set; }
    }

    [Serializable]
    public class IndustryIdentifier
    {
        public string type { get; set; }
        public string identifier { get; set; }
    }

    [Serializable]
    public class ImageLink
    {
        public string smallThumbnail { get; set; }
        public string thumbnail { get; set; }
    }

}