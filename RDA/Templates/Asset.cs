﻿using RDA.Data;
using RDA.Library;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace RDA.Templates {

  public class Asset {

    #region Fields

    public String Path = String.Empty;

    #endregion Fields

    #region Constructors

    public Asset(XElement asset, Boolean findSources) {
      foreach (var element in asset.Element("Values").Elements()) {
        switch (element.Name.LocalName) {
          case "Text":
          case "Locked":
          case "Buff":
          case "Cost":
          case "Blocking":
          case "Building":
          case "Selection":
          case "Object":
          case "Constructable":
          case "Mesh":
          case "SoundEmitter":
          case "FeedbackController":
          case "AmbientMoodProvider":
          case "Pausable":
          case "BuildPermit":
          case "VisualEffectWhenActive":
            // ignore this nodes
            break;

          case "Standard":
            this.ProcessElement_Standard(element);
            break;

          case "Item":
            this.ProcessElement_Item(element);
            break;

          case "ItemEffect":
            this.ProcessElement_ItemEffect(element);
            break;

          case "FactoryUpgrade":
            this.ProcessElement_FactoryUpgrade(element);
            break;

          case "BuildingUpgrade":
            this.ProcessElement_BuildingUpgrade(element);
            break;

          case "CultureUpgrade":
            this.ProcessElement_CultureUpgrade(element);
            break;

          case "ModuleOwnerUpgrade":
            this.ProcessElement_ModuleOwnerUpgrade(element);
            break;

          case "ResidenceUpgrade":
            this.ProcessElement_ResidenceUpgrade(element);
            break;

          case "PopulationUpgrade":
            this.ProcessElement_PopulationUpgrade(element);
            break;

          case "ElectricUpgrade":
            this.ProcessElement_ElectricUpgrade(element);
            break;

          case "ExpeditionAttribute":
            this.ProcessElement_ExpeditionAttribute(element);
            break;

          case "VisitorHarborUpgrade":
            this.ProcessElement_VisitorHarborUpgrade(element);
            break;

          case "AttackerUpgrade":
            this.ProcessElement_AttackerUpgrade(element);
            break;

          case "ShipyardUpgrade":
            this.ProcessElement_ShipyardUpgrade(element);
            break;

          case "AttackableUpgrade":
            this.ProcessElement_AttackableUpgrade(element);
            break;

          case "ProjectileUpgrade":
            this.ProcessElement_ProjectileUpgrade(element);
            break;

          case "VehicleUpgrade":
            this.ProcessElement_VehicleUpgrade(element);
            break;

          case "RepairCraneUpgrade":
            this.ProcessElement_RepairCraneUpgrade(element);
            break;

          case "KontorUpgrade":
            this.ProcessElement_KontorUpgrade(element);
            break;

          case "TradeShipUpgrade":
            this.ProcessElement_TradeShipUpgrade(element);
            break;

          case "ItemAction":
            this.ProcessElement_ItemActions(element);
            break;

          case "PassiveTradeGoodGenUpgrade":
            this.ProcessElement_PassiveTradeGoodGenUpgrades(element);
            break;

          case "IncidentInfectableUpgrade":
            this.ProcessElement_IncidentInfectableUpgrades(element);
            break;

          case "IncidentInfluencerUpgrade":
            this.ProcessElement_IncidentInfluencerUpgrades(element);
            break;

          case "ItemGeneratorUpgrade":
            this.ProcessElement_ItemGeneratorUpgrades(element);
            break;

          case "SpecialAction":
            this.ProcessElement_SpecialActions(element);
            break;

          case "MonumentEventCategory":
            this.ProcessElement_MonumentEventCategory(element);
            break;

          case "MonumentEvent":
            this.ProcessElement_MonumentEvent(element);
            break;

          case "Ornament":
            this.Info = element.Element("OrnamentDescritpion") == null ? null : new Description(element.Element("OrnamentDescritpion").Value);
            break;

          case "Reward":
            this.ProcessElement_MonumentEventReward(element);
            break;
          case "Product":
            //Todo: maybe add some properties
            break;
          default:
            throw new NotImplementedException(element.Name.LocalName);
        }
      }
      if (findSources) {
        var sources = this.FindSources(this.ID).ToArray();
        this.Sources = sources.Select(s => new TempSource(s)).ToList();
      }
    }

    #endregion Constructors

    #region Properties

    public static ConcurrentDictionary<string, SourceWithDetailsList> SavedSources { get; set; } = new ConcurrentDictionary<string, SourceWithDetailsList>();
    public String ID { get; set; }
    public String Name { get; set; }
    public Icon Icon { get; set; }
    public Description Text { get; set; }
    public Description Rarity { get; set; }
    public String ItemType { get; set; }
    public String ReleaseVersion { get; set; } = "Release";

    //
    public Allocation Allocation { get; set; }

    //
    public List<Description> EffectTargets { get; set; }

    //
    public List<Upgrade> FactoryUpgrades { get; set; }

    public List<Upgrade> BuildingUpgrades { get; set; }
    public List<Upgrade> CultureUpgrades { get; set; }
    public List<Upgrade> ModuleOwnerUpgrades { get; set; }
    public List<Upgrade> ResidenceUpgrades { get; set; }
    public List<Upgrade> PopulationUpgrades { get; set; }
    public List<Upgrade> ElectricUpgrades { get; set; }
    public List<Upgrade> ExpeditionAttributes { get; set; }
    public List<Upgrade> AttackableUpgrades { get; set; }
    public List<Upgrade> TradeShipUpgrades { get; set; }
    public List<Upgrade> VehicleUpgrades { get; set; }
    public List<Upgrade> AttackerUpgrades { get; set; }
    public List<Upgrade> VisitorHarborUpgrades { get; set; }
    public List<Upgrade> RepairCraneUpgrades { get; set; }
    public List<Upgrade> IncidentInfectableUpgrades { get; set; }
    public List<Upgrade> IncidentInfluencerUpgrades { get; set; }
    public List<Upgrade> ItemGeneratorUpgrades { get; set; }

    //
    public List<Upgrade> ItemSets { get; set; }

    //
    public String TradePrice { get; set; }

    //
    public Description Info { get; set; }

    //
    public List<TempSource> Sources { get; set; }

    //
    public List<String> MonumentEvents { get; set; }

    public List<String> MonumentThresholds { get; set; }
    public List<String> MonumentRewards { get; set; }

    #endregion Properties

    #region Methods

    public XElement ToXml() {
      var result = new XElement(this.GetType().Name);
      result.Add(new XAttribute("ID", this.ID));
      result.Add(new XAttribute("Release", this.ReleaseVersion));
      result.Add(new XElement("Name", this.Name));
      result.Add(this.Icon.ToXml());
      result.Add(this.Text.ToXml("Text"));
      result.Add(this.Rarity == null ? new XElement("Rarity") : this.Rarity.ToXml("Rarity"));
      result.Add(new XElement("ItemType", this.ItemType));
      result.Add(new XElement("ItemSets", this.ItemSets?.Select(s => s.ToXml())));
      //
      result.Add(this.Rarity == null ? new XElement("Allocation") : this.Allocation.ToXml());
      //
      result.Add(new XElement("EffectTargets", this.EffectTargets == null ? null : this.EffectTargets.Select(s => s.ToXml("Target"))));
      //
      result.Add(new XElement("FactoryUpgrades", this.FactoryUpgrades == null ? null : this.FactoryUpgrades.Select(s => s.ToXml())));
      result.Add(new XElement("BuildingUpgrades", this.BuildingUpgrades == null ? null : this.BuildingUpgrades.Select(s => s.ToXml())));
      result.Add(new XElement("CultureUpgrades", this.CultureUpgrades == null ? null : this.CultureUpgrades.Select(s => s.ToXml())));
      result.Add(new XElement("ModuleOwnerUpgrades", this.ModuleOwnerUpgrades == null ? null : this.ModuleOwnerUpgrades.Select(s => s.ToXml())));
      result.Add(new XElement("ResidenceUpgrades", this.ResidenceUpgrades == null ? null : this.ResidenceUpgrades.Select(s => s.ToXml())));
      result.Add(new XElement("PopulationUpgrades", this.PopulationUpgrades == null ? null : this.PopulationUpgrades.Select(s => s.ToXml())));
      result.Add(new XElement("ElectricUpgrades", this.ElectricUpgrades == null ? null : this.ElectricUpgrades.Select(s => s.ToXml())));
      result.Add(new XElement("ExpeditionAttributes", this.ExpeditionAttributes == null ? null : this.ExpeditionAttributes.Select(s => s.ToXml())));
      result.Add(new XElement("AttackableUpgrades", this.AttackableUpgrades == null ? null : this.AttackableUpgrades.Select(s => s.ToXml())));
      result.Add(new XElement("TradeShipUpgrades", this.TradeShipUpgrades == null ? null : this.TradeShipUpgrades.Select(s => s.ToXml())));
      result.Add(new XElement("VehicleUpgrades", this.VehicleUpgrades == null ? null : this.VehicleUpgrades.Select(s => s.ToXml())));
      result.Add(new XElement("AttackerUpgrades", this.AttackerUpgrades == null ? null : this.AttackerUpgrades.Select(s => s.ToXml())));
      result.Add(new XElement("VisitorHarborUpgrades", this.VisitorHarborUpgrades == null ? null : this.VisitorHarborUpgrades.Select(s => s.ToXml())));
      result.Add(new XElement("RepairCraneUpgrades", this.RepairCraneUpgrades == null ? null : this.RepairCraneUpgrades.Select(s => s.ToXml())));
      result.Add(new XElement("IncidentInfectableUpgrades", this.IncidentInfectableUpgrades == null ? null : this.IncidentInfectableUpgrades.Select(s => s.ToXml())));
      result.Add(new XElement("IncidentInfluencerUpgrades", this.IncidentInfluencerUpgrades == null ? null : this.IncidentInfluencerUpgrades.Select(s => s.ToXml())));
      result.Add(new XElement("ItemGeneratorUpgrades", this.ItemGeneratorUpgrades == null ? null : this.ItemGeneratorUpgrades.Select(s => s.ToXml())));
      //
      result.Add(new XElement("TradePrice", this.TradePrice));
      //
      if (this.Info != null)
        result.Add(this.Info.ToXml("Info"));
      //
      result.Add(new XElement("Sources", this.Sources?.Select(s => s.ToXml())));
      //
      result.Add(new XElement("MonumentEvents", this.MonumentEvents == null ? null : this.MonumentEvents.Select(s => new XElement("Event", s))));
      result.Add(new XElement("MonumentThresholds", this.MonumentThresholds == null ? null : this.MonumentThresholds.Select(s => new XElement("Threshold", s))));
      result.Add(new XElement("MonumentRewards", this.MonumentRewards == null ? null : this.MonumentRewards.Select(s => new XElement("Reward", s))));
      return result;
    }
    public override String ToString() {
      return $"{this.ID} - {this.Name}";
    }

    private void ProcessElement_Standard(XElement element) {
      this.ID = element.Element("GUID").Value;
      this.Name = element.Element("Name").Value;
      this.Icon = new Icon(element.Element("IconFilename").Value);
      this.Text = new Description(element.Element("GUID").Value);
      this.Info = element.Element("InfoDescription") == null ? null : new Description(element.Element("InfoDescription").Value);
    }
    private void ProcessElement_Item(XElement element) {
      this.Rarity = element.Element("Rarity") == null ? new Description("118002") : new Description(Helper.GetDescriptionID(element.Element("Rarity").Value));
      this.ItemType = element.Element("ItemType")?.Value ?? "Common";
      this.Allocation = new Allocation(element.Parent.Parent.Element("Template").Value, element.Element("Allocation")?.Value);
      this.TradePrice = element.Element("TradePrice") == null ? null : (Int32.Parse(element.Element("TradePrice").Value) / 4).ToString();
      if (element.Element("ItemSet") != null) {
        this.ItemSets = new List<Upgrade>();
        this.ItemSets.Add(new Upgrade(element.Element("ItemSet")));
      }
      if (this.ItemType == "None")
        this.ItemType = "Common";
      if (this.ItemType == "Normal")
        this.ItemType = "Common";
      if (this.ItemType != "Specialist" && this.ItemType != "Common")
        throw new NotImplementedException();
    }
    private void ProcessElement_ItemEffect(XElement element) {
      if (element.HasElements && element.Element("EffectTargets") == null)
        throw new NotImplementedException();
      if (element.HasElements && element.Element("EffectTargets").HasElements) {
        this.EffectTargets = new List<Description>();
        foreach (var item in element.Element("EffectTargets").Elements()) {
          this.EffectTargets.Add(new Description(Program.DescriptionEN[item.Value], Program.DescriptionDE[item.Value]));
        }
      }
    }
    private void ProcessElement_FactoryUpgrade(XElement element) {
      if (element.HasElements) {
        this.FactoryUpgrades = new List<Upgrade>();
        foreach (var item in element.Elements()) {
          this.FactoryUpgrades.Add(new Upgrade(item));
        }
      }
    }
    private void ProcessElement_BuildingUpgrade(XElement element) {
      if (element.HasElements) {
        this.BuildingUpgrades = new List<Upgrade>();
        foreach (var item in element.Elements()) {
          // TODO: this needs to be implemented
          if (item.Name.LocalName == "ResolverUnitCountUpgrade")
            continue;
          if (item.Name.LocalName == "PublicServiceNoSatisfactionDistance")
            continue;
          if (item.Name.LocalName == "ResolverUnitMovementSpeedUpgrade")
            continue;
          if (item.Name.LocalName == "ResolverUnitDecreaseUpgrade")
            continue;
          this.BuildingUpgrades.Add(new Upgrade(item));
        }
      }
    }
    private void ProcessElement_CultureUpgrade(XElement element) {
      if (element.HasElements) {
        this.CultureUpgrades = new List<Upgrade>();
        foreach (var item in element.Elements()) {
          if (item.Name.LocalName == "ChangeModule")
            continue;
          if (item.Name.LocalName == "ForcedFeedbackVariation")
            continue;
          if (item.Name.LocalName == "AdditionalModuleSoundLoop")
            continue;
          this.CultureUpgrades.Add(new Upgrade(item));
        }
      }
    }
    private void ProcessElement_ModuleOwnerUpgrade(XElement element) {
      if (element.HasElements) {
        this.ModuleOwnerUpgrades = new List<Upgrade>();
        foreach (var item in element.Elements()) {
          this.ModuleOwnerUpgrades.Add(new Upgrade(item));
        }
      }
    }
    private void ProcessElement_ResidenceUpgrade(XElement element) {
      if (element.HasElements) {
        this.ResidenceUpgrades = new List<Upgrade>();
        foreach (var item in element.Elements()) {
          // TODO: this needs to be implemented
          if (item.Name.LocalName == "ChangedSupplyValueUpgrade")
            continue;
          this.ResidenceUpgrades.Add(new Upgrade(item));
        }
      }
    }
    private void ProcessElement_PopulationUpgrade(XElement element) {
      if (element.HasElements) {
        this.PopulationUpgrades = new List<Upgrade>();
        foreach (var item in element.Elements()) {
          switch (item.Name.LocalName) {
            case "InputBenefitModifier":
              foreach (var subItem in item.XPathSelectElements("Item/*")) {
                switch (subItem.Name.LocalName) {
                  case "Product":
                    // ignore
                    break;

                  case "AdditionalSupply":
                    // TODO: this needs to be implemented
                    break;

                  case "AdditionalHappiness":
                  case "AdditionalMoney":
                    this.PopulationUpgrades.Add(new Upgrade(subItem));
                    break;

                  default:
                    throw new NotImplementedException(subItem.Name.LocalName);
                }
              }
              break;

            default:
              this.PopulationUpgrades.Add(new Upgrade(item));
              break;
          }
        }
      }
    }
    private void ProcessElement_ElectricUpgrade(XElement element) {
      if (element.HasElements) {
        this.ElectricUpgrades = new List<Upgrade>();
        foreach (var item in element.Elements()) {
          this.ElectricUpgrades.Add(new Upgrade(item));
        }
      }
    }
    private void ProcessElement_ExpeditionAttribute(XElement element) {
      if (element.HasElements) {
        var attributes = element.XPathSelectElements("ExpeditionAttributes/Item").Where(w => w.HasElements).ToArray();
        if (attributes.Length > 0) {
          this.ExpeditionAttributes = new List<Upgrade>();
          foreach (var attribute in attributes) {
            if (attribute.Element("Attribute") == null)
              continue;
            if (attribute.Element("Attribute").Value == "PerkFemale")
              continue;
            if (attribute.Element("Attribute").Value == "PerkMale")
              continue;
            if (attribute.Element("Attribute").Value == "PerkEntertainer")
              continue;
            this.ExpeditionAttributes.Add(new Upgrade(attribute.Element("Attribute").Value, attribute.Element("Amount")?.Value));
          }
        }
      }
    }
    private void ProcessElement_VisitorHarborUpgrade(XElement element) {
      if (element.HasElements) {
        this.VisitorHarborUpgrades = new List<Upgrade>();
        foreach (var item in element.Elements()) {
          this.VisitorHarborUpgrades.Add(new Upgrade(item));
        }
      }
    }
    private void ProcessElement_PassiveTradeGoodGenUpgrades(XElement element) {
      if (element.HasElements) {
        // TODO: this needs to be implemented
      }
    }
    private void ProcessElement_IncidentInfectableUpgrades(XElement element) {
      if (element.HasElements) {
        this.IncidentInfectableUpgrades = new List<Upgrade>();
        foreach (var item in element.Elements()) {
          // TODO: this needs to be implemented
          if (item.Name.LocalName == "IncidentIllnessIncreaseUpgrade")
            continue;
          if (item.Name.LocalName == "OverrideIncidentAttractiveness")
            continue;
          this.IncidentInfectableUpgrades.Add(new Upgrade(item));
        }
      }
    }
    private void ProcessElement_IncidentInfluencerUpgrades(XElement element) {
      if (element.HasElements) {
        // TODO: this needs to be implemented
      }
    }
    private void ProcessElement_AttackerUpgrade(XElement element) {
      if (element.HasElements) {
        this.AttackerUpgrades = new List<Upgrade>();
        foreach (var item in element.Elements()) {
          // TODO: this needs to be implemented
          if (item.Name.LocalName == "LineOfSightRangeUpgrade")
            continue;
          if (item.Name.LocalName == "HitpointDamage")
            continue;
          if (item.Name.LocalName == "ReloadTimeUpgrade")
            continue;
          if (item.Name.LocalName == "MoraleDamage")
            continue;
          if (item.Name.LocalName == "AddStatusEffects")
            continue;
          if (item.Name.LocalName == "DamageFactor") {
            foreach (var factor in item.Elements()) {
              this.AttackerUpgrades.Add(new Upgrade(factor));
            }
            continue;
          }
          this.AttackerUpgrades.Add(new Upgrade(item));
        }
      }
    }
    private void ProcessElement_ShipyardUpgrade(XElement element) {
      if (element.HasElements) {
        // TODO: this needs to be implemented
      }
    }
    private void ProcessElement_AttackableUpgrade(XElement element) {
      if (element.HasElements) {
        this.AttackableUpgrades = new List<Upgrade>();
        foreach (var item in element.Elements()) {
          switch (item.Name.LocalName) {
            case "DamageReceiveFactor":
              foreach (var subItem in item.Elements()) {
                // TODO: this needs to be implemented
                if (subItem.Name.LocalName == "Normal")
                  continue;
                this.AttackableUpgrades.Add(new Upgrade(subItem));
              }
              break;

            case "MoralePowerUpgrade":
              // TODO: this needs to be implemented
              break;

            default:
              this.AttackableUpgrades.Add(new Upgrade(item));
              break;
          }
        }
      }
    }
    private void ProcessElement_ProjectileUpgrade(XElement element) {
      if (element.HasElements) {
        throw new NotImplementedException();
      }
    }
    private void ProcessElement_VehicleUpgrade(XElement element) {
      if (element.HasElements) {
        this.VehicleUpgrades = new List<Upgrade>();
        foreach (var item in element.Elements()) {
          // TODO: this needs to be implemented
          if (item.Name.LocalName == "MaintainanceUpgrade")
            continue;
          this.VehicleUpgrades.Add(new Upgrade(item));
        }
      }
    }
    private void ProcessElement_RepairCraneUpgrade(XElement element) {
      if (element.HasElements) {
        this.RepairCraneUpgrades = new List<Upgrade>();
        foreach (var item in element.Elements()) {
          // TODO: this needs to be implemented
          if (item.Name.LocalName == "HealBuildingsPerMinuteUpgrade")
            continue;
          this.RepairCraneUpgrades.Add(new Upgrade(item));
        }
      }
    }
    private void ProcessElement_KontorUpgrade(XElement element) {
      if (element.HasElements) {
        // TODO: this needs to be implemented
      }
    }
    private void ProcessElement_TradeShipUpgrade(XElement element) {
      if (element.HasElements) {
        this.TradeShipUpgrades = new List<Upgrade>();
        foreach (var item in element.Elements()) {
          this.TradeShipUpgrades.Add(new Upgrade(item));
        }
      }
    }
    private void ProcessElement_ItemActions(XElement element) {
      if (element.HasElements) {
        // TODO: this needs to be implemented
      }
    }
    private void ProcessElement_ItemGeneratorUpgrades(XElement element) {
      if (element.HasElements) {
        // TODO: this needs to be implemented
      }
    }
    private void ProcessElement_SpecialActions(XElement element) {
      if (element.HasElements) {
        // TODO: this needs to be implemented
      }
    }
    private void ProcessElement_MonumentEventCategory(XElement element) {
      this.MonumentEvents = element.XPathSelectElements("Events/Item/Event").Select(s => s.Value).ToList();
    }
    private void ProcessElement_MonumentEvent(XElement element) {
      this.MonumentThresholds = element.XPathSelectElements("RewardThresholds/Item/Reward").Select(s => s.Value).ToList();
    }
    private void ProcessElement_MonumentEventReward(XElement element) {
      this.MonumentRewards = element.XPathSelectElements("RewardAssets/Item/Reward").Select(s => s.Value).ToList();
    }
    private SourceWithDetailsList FindSources(String id, Details mainDetails = default, SourceWithDetailsList inResult = default) {
      mainDetails = (mainDetails == default) ? new Details() : mainDetails;
      mainDetails.PreviousIDs.Add(id);
      var mainResult = inResult ?? new SourceWithDetailsList();
      var resultstoadd = new List<SourceWithDetailsList>();
      var links = Program.Original.Root.XPathSelectElements($"//*[text()={id} and not(self::GUID)]").ToArray();
      if (links.Length > 0) {
        for (var i = 0; i < links.Length; i++) {
          var element = links[i];
          var isShipDrop = element.Name.LocalName == "ShipDropRewardPool";
          while (element.Name.LocalName != "Asset" || !element.HasElements) {
            element = element.Parent;
          }

          var Details = new Details(mainDetails);
          var result = mainResult.Copy();
          var key = element.XPathSelectElement("Values/Standard/GUID").Value;

          if (element.Element("Template") == null || isShipDrop) {
            // Ship Drop
            if (element.XPathSelectElement("Values/Profile/ShipDropRewardPool")?.Value == id) {
              result.AddSourceAsset(element.GetProxyElement("ShipDrop"), new HashSet<XElement> { element.GetProxyElement("ShipDrop") });
              resultstoadd.Add(result);
            }
            // Hafen Hugo Mercier
            if (element.XPathSelectElement("Values/Standard/GUID")?.Value == "220") {
              if (element.XPathSelectElements("Values/ConstructionAI/ItemTradeConfig/ItemPools").Elements().Any(f => f.Element("Pool").Value == id)) {
                result.AddSourceAsset(element.GetProxyElement("HafenHugo"), new HashSet<XElement> { element.GetProxyElement("HafenHugo") });
                resultstoadd.Add(result);
              }
            }
            continue;
          }
          if (mainDetails.PreviousIDs.Contains(key)) {
            continue;
          }

          switch (element.Element("Template").Value) {
            case "AssetPool":
            case "TutorialQuest":
            case "SettlementRightsFeature":
            case "Profile_2ndParty":
            case "GuildhouseItem":
            case "HarborOfficeItem":
            case "HarbourOfficeBuff":
            case "MonumentEvent":
            case "MainQuest":
              // ignore
              break;

            case "Expedition":
              if (!element.XPathSelectElement("Values/Standard/Name").Value.Contains("Test")) {
                if (Details.Items.Count == 0) {
                  Details.Add(element);
                }
                result.AddSourceAsset(element, Details.Items);
              }
              break;

            case "Profile_3rdParty":
            case "Profile_3rdParty_Pirate":
              if (!element.XPathSelectElement("Values/Standard/Name").Value.Contains("Test")
                  && element.XPathSelectElement("Standard/Profile/ShipDropRewardPool")?.Value == id) {
                result.AddSourceAsset(element.GetProxyElement("ShipDrop"), new HashSet<XElement> { element.GetProxyElement("ShipDrop") });
                break;
              }
              goto case "A7_QuestEscortObject";
            case "A7_QuestEscortObject":
            case "A7_QuestDeliveryObject":
            case "A7_QuestDestroyObjects":
            case "A7_QuestPickupObject":
            case "A7_QuestFollowShip":
            case "A7_QuestPhotography":
            case "A7_QuestStatusQuo":
            case "A7_QuestItemUsage":
            case "A7_QuestSustain":
            case "A7_QuestPicturePuzzleObject":
            case "Quest":
            case "CollectablePicturePuzzle":
            case "MonumentEventReward":
            case "A7_QuestSmuggler":
              if (!element.XPathSelectElement("Values/Standard/Name").Value.Contains("Test")) {
                result.AddSourceAsset(element, new HashSet<XElement> { element });
              }
              break;
            case "TourismFeature":
              var pool = element.Descendants("Pool").FirstOrDefault(p => p.Value == id)?.Parent;
              if (pool != null) {
                result.AddSourceAsset(element, new HashSet<XElement> { pool });
              }
              break;
            case "ExpeditionDecision":
              //Add Detail if has reward or is InsertEvent
              var reward = element.XPathSelectElement("Values/Reward/RewardAssets");
              if ((reward?.Elements("Item").Any(l => l.Element("Reward").Value == id) ?? false)
                  || element.XPathSelectElements($"//*[text()={id} and not(self::GUID) and (self::InsertEvent)]").Any()) {
                Details.Add(element);
              }
              goto case "ExpeditionOption";
            case "ExpeditionOption":
            case "ExpeditionTrade":
            case "ExpeditionEvent":
            case "ExpeditionEventPool":
              if (SavedSources.ContainsKey(key)) {
                result.AddSourceAsset(SavedSources[key].Copy(), Details);
                break;
              }
              goto case "RewardPool";
            case "RewardPool":
            case "RewardItemPool":

              if (SavedSources.ContainsKey(key)) {
                result.AddSourceAsset(SavedSources[key].Copy());
                break;
              }
              var weight = element.Descendants("Item").FirstOrDefault(f => f.Elements().Any(l => l.Value == id))?.Element("Weight")?.Value;
              if (weight == "0") {
                break;
              }
              FindSources(key, Details, result);
              if (!SavedSources.ContainsKey(key)) {
                SavedSources.TryAdd(key, result.Copy());
              }
              break;

            default:
              //throw new NotImplementedException(element.Element("Template").Value);
              Debug.WriteLine(element.Element("Template").Value);
              break;
          }
          resultstoadd.Add(result);
        }
      }

      foreach (var item in resultstoadd) {
        mainResult.AddSourceAsset(item);
      }
      return mainResult;
    }

    #endregion Methods
  }
}