﻿namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Physicist.Controls;
    using Physicist.Enums;

    public class PathNode : IXmlSerializable
    {
        private bool isActive;

        private Dictionary<TriggerMode, Dictionary<string, IModifier>> modifiers = new Dictionary<TriggerMode, Dictionary<string, IModifier>>();

        public PathNode()
        {
            this.IsInitialized = false;
            this.modifiers.Add(TriggerMode.OnActivated, new Dictionary<string, IModifier>());
            this.modifiers.Add(TriggerMode.WhileActivated, new Dictionary<string, IModifier>());
            this.modifiers.Add(TriggerMode.OnDeactivated, new Dictionary<string, IModifier>());
        }

        public PathNode(XElement element)
        {
            this.XmlDeserialize(element);
        }

        public PathNode(Actor target)
        {
            this.TargetActor = target;
        }

        public event EventHandler<EventArgs> Deactivated;

        public bool IsInitialized { get; set; }

        public bool IsActive
        {
            get
            {
                return this.isActive;
            }

            set
            {
                if (this.isActive != value)
                {
                    this.isActive = value;
                    if (this.isActive)
                    {
                        foreach (var mode in this.modifiers.Keys)
                        {
                            foreach (var modifier in this.modifiers[mode].Values)
                            {
                                modifier.IsActive = !modifier.IsActive;
                            }
                        }
                    }

                    if (!this.isActive && this.Deactivated != null)
                    {
                        this.Deactivated(this, null);
                    }
                }
            }
        }

        public Actor TargetActor { get; set; }

        protected Dictionary<TriggerMode, Dictionary<string, IModifier>> Modifiers 
        {
            get
            {
                return this.modifiers;
            }

            set
            {
                this.modifiers = value;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (this.isActive)
            {
                foreach (var modifier in this.modifiers[TriggerMode.WhileActivated].Values)
                {
                    if (modifier != null)
                    {
                        modifier.Update(gameTime);
                    }
                }
            }
        }

        public void Initialize(IEnumerable<IModifier> availableModifiers)
        {
            if (availableModifiers != null)
            {
                foreach (var modifier in availableModifiers)
                {
                    foreach (var mode in this.modifiers.Keys)
                    {
                        if (this.modifiers[mode].ContainsKey(modifier.Name))
                        {
                            this.modifiers[mode][modifier.Name] = modifier;
                            modifier.IsActive = mode == TriggerMode.OnDeactivated;
                        }
                    }
                }

                this.IsInitialized = true;
            }
        }

        public void AddModifier(TriggerMode mode, IModifier modifier)
        {
            if (modifier != null)
            {
                this.modifiers[mode].Add(modifier.Name, modifier);
            }
        }

        public bool RemoveModifier(IModifier modifier)
        {
            bool removed = false;
            foreach (var mode in this.modifiers.Keys)
            {
                foreach (var mod in this.modifiers[mode].Values)
                {
                    if (mod.Name == modifier.Name)
                    {
                        this.modifiers[mode].Remove(modifier.Name);
                        removed = true;
                    }
                }
            }

            return removed;
        }

        public virtual XElement XmlSerialize()
        {
            throw new NotImplementedException();
        }

        public virtual void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                var modifiersEle = element.Element("Modifiers");

                if (modifiersEle != null)
                {
                    foreach (var mode in this.modifiers.Keys)
                    {
                        var modeEle = element.Element(mode.ToString());
                        if (modeEle != null)
                        {
                            foreach (var modifierEle in modeEle.Elements("Modifier"))
                            {
                                this.modifiers[mode].Add(modifierEle.Attribute("name").Value, null);
                            }
                        }
                    }
                }
            }
        }
    }
}
