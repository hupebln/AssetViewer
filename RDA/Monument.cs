﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using RDA.Templates;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo
// ReSharper disable CoVariantArrayConversion
namespace RDA {

  /// <summary>
  ///   World Fair
  /// </summary>
  public static class Monument {

    #region Public Methods
    public static void Create() {
      Monument.MonumentEventCategory();
      Monument.MonumentEvent();
      Monument.MonumentThreshold();
    }
    #endregion

    #region Private Methods
    private static void MonumentEventCategory() {
      var result = new List<Asset>();
      var monumentCategories = Program.Original.Root.XPathSelectElements("//Asset[Template='MonumentEventCategory']").ToArray();
      foreach (var monumentCategory in monumentCategories) {
        Console.WriteLine(monumentCategory.XPathSelectElement("Values/Standard/GUID").Value);
        var asset = new Asset(monumentCategory, false);
        result.Add(asset);
      }
      var document = new XDocument();
      document.Add(new XElement("MonumentCategory"));
      document.Root.Add(result.Select(s => s.ToXml()));
      document.Save($@"{Program.PathRoot}\Modified\Assets_MonumentCategory.xml");
      document.Save($@"{Program.PathViewer}\Resources\Assets\MonumentCategory.xml");
    }
    private static void MonumentEvent() {
      var result = new List<Asset>();
      var monumentEvents = Program.Original.Root.XPathSelectElements("//Asset[Template='MonumentEvent']").ToArray();
      foreach (var monumentEvent in monumentEvents) {
        Console.WriteLine(monumentEvent.XPathSelectElement("Values/Standard/GUID").Value);
        var asset = new Asset(monumentEvent, false);
        result.Add(asset);
      }
      var document = new XDocument();
      document.Add(new XElement("MonumentEvent"));
      document.Root.Add(result.Select(s => s.ToXml()));
      document.Save($@"{Program.PathRoot}\Modified\Assets_MonumentEvent.xml");
      document.Save($@"{Program.PathViewer}\Resources\Assets\MonumentEvent.xml");
    }
    private static void MonumentThreshold() {
      var result = new List<Asset>();
      var monumentEvents = Program.Original.Root.XPathSelectElements("//Asset[Template='MonumentEventReward']").ToArray();
      foreach (var monumentEvent in monumentEvents) {
        Console.WriteLine(monumentEvent.XPathSelectElement("Values/Standard/GUID").Value);
        var asset = new Asset(monumentEvent, false);
        result.Add(asset);
      }
      var document = new XDocument();
      document.Add(new XElement("MonumentEvent"));
      document.Root.Add(result.Select(s => s.ToXml()));
      document.Save($@"{Program.PathRoot}\Modified\Assets_MonumentThreshold.xml");
      document.Save($@"{Program.PathViewer}\Resources\Assets\MonumentThreshold.xml");
    }
    #endregion

  }

}