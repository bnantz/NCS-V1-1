using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace NCS.Windows.Forms
{
   [ProvideProperty("DirtyCheckingEnabled", typeof(Control))]
   public class DirtyChecker : Component, IExtenderProvider
   {
      // Events
      public event DirtyChangedEventHandler DirtyChanged;

      // Methods
      public DirtyChecker()
      {
         this.m_blnExternalDirty = false;
         this.m_bSuspendDirtyChecking = false;
         this.mbLastDirty = false;
         this.mbIncludeDisabledInvisibleControls = true;
         this.m_bDirtyCheckingStarted = false;
         this.mbExternalSuspended = false;
         this.InitializeComponent();
         this.colControlInformation = new Hashtable();
      }

      public DirtyChecker(IContainer Container) : this()
      {
         Container.Add(this);
      }

      public void Cancel()
      {
         this.m_bDirtyCheckingStarted = false;
         this.CheckForDirtyChange(this);
      }

      public bool CanExtend(object target)
      {
         if (!(target is Control))
         {
            return false;
         }
         if ((((target is TreeView) | (target is PictureBox)) | (target is ListView)) | (target.GetType().ToString().ToUpper().IndexOf("GRID") != 0))
         {
            return false;
         }
         return true;
      }

      public void CheckForDirtyChange(object sender)
      {
         if (!(this.m_bSuspendDirtyChecking | this.mbExternalSuspended))
         {
            bool bCurrentDirty = this.IsDirty | this.m_blnExternalDirty;
            if (bCurrentDirty != this.LastDirty)
            {
               this.LastDirty = bCurrentDirty;
               DirtyChangedEventArgs e = new DirtyChangedEventArgs();
               e.Dirty = bCurrentDirty;
               if (this.DirtyChanged != null)
               {
                  this.DirtyChanged(this, e);
               }
            }
         }
      }

      public void CommonHandler(object sender, EventArgs e)
      {
         this.CheckForDirtyChange(RuntimeHelpers.GetObjectValue(sender));
      }

      private string ConstructIndices(ListBox ctlListBox)
      {
         string sIndices="";
         foreach(int nIndex in ctlListBox.SelectedIndices)
         {
            sIndices += nIndex.ToString() + "~";
         }
         return sIndices;
      }

      public bool ControlIsDirty(Control ctrl)
      {
         if (this.colControlInformation.Contains(ctrl))
         {
            ControlInformationHolder objField = (ControlInformationHolder) this.colControlInformation[ctrl];
            return this.FieldIsDirty(objField, ctrl);
         }
         return false;
      }

      public void DateChangedHandler(object sender, DateRangeEventArgs e)
      {
         this.CheckForDirtyChange(RuntimeHelpers.GetObjectValue(sender));
      }

      protected override void Dispose(bool disposing)
      {
         if (disposing && (this.components != null))
         {
            this.components.Dispose();
         }
         base.Dispose(disposing);
      }

      private bool FieldIsDirty(ControlInformationHolder objField, Control ctl)
      {
         bool bReturn = false;

         if (!this.m_bDirtyCheckingStarted)
         {
            return false;
         }
         if (!objField.DirtyCheckingEnabled)
         {
            return false;
         }
         switch (objField.ControlType)
         {
            case ControlType.typeTextbox:
            {
               if (String.Compare(objField.StringValue, ctl.Text, false) == 0)
               {
                  return false;
               }
               return true;
            }
            case ControlType.typeDatePicker:
            {
               DateTimePicker picker1 = (DateTimePicker) ctl;
               if ((objField.BooleanValue == picker1.Checked) && (String.Compare(objField.StringValue, picker1.Value.ToLongDateString(), false) == 0))
               {
                  return false;
               }
               return true;
            }
            case ControlType.typeMonthCalendar:
            {
               MonthCalendar calendar1 = (MonthCalendar) ctl;
               if ((DateTime.Compare(objField.DateValue, calendar1.SelectionStart) == 0) & (DateTime.Compare(objField.EndDateValue, calendar1.SelectionEnd) == 0))
               {
                  return false;
               }
               return true;
            }
            case ControlType.typeRadioButton:
            {
               RadioButton button1 = (RadioButton) ctl;
               if (objField.BooleanValue == button1.Checked)
               {
                  return false;
               }
               return true;
            }
            case ControlType.typeCheckBox:
            {
               CheckBox box1 = (CheckBox) ctl;
               if ((objField.CheckStateValue == box1.CheckState) && (objField.BooleanValue == box1.Checked))
               {
                  return false;
               }
               return true;
            }
            case ControlType.typeListBox:
            {
               ListBox box3 = (ListBox) ctl;
               if (box3.SelectionMode != SelectionMode.One)
               {
                  string text1 = this.ConstructIndices(box3);
                  if (String.Compare(objField.StringValue, text1, false) == 0)
                  {
                     return false;
                  }
                  return true;
               }
               if (objField.IntegerValue == box3.SelectedIndex)
               {
                  return false;
               }
               return true;
            }
            case ControlType.typeComboBox:
            {
               ComboBox box2 = (ComboBox) ctl;
               if (box2.DropDownStyle != ComboBoxStyle.DropDownList)
               {
                  if (String.Compare(objField.StringValue, box2.Text, false) == 0)
                  {
                     return false;
                  }
                  return true;
               }
               if (objField.IntegerValue == box2.SelectedIndex)
               {
                  return false;
               }
               return true;
            }
            case ControlType.typeRichTextBox:
            {
               RichTextBox box4 = (RichTextBox) ctl;
               if (objField.IntegerValue == (box4.Text + box4.Rtf).GetHashCode())
               {
                  return false;
               }
               return true;
            }
            case ControlType.typeOther:
            {
               if (DirtyChecker.CustomIsDirtyContainer != null)
               {
                  if (DirtyChecker.CustomIsDirtyContainer.ControlSupported(ctl))
                  {
                     return DirtyChecker.CustomIsDirtyContainer.CustomIsDirty(ctl);
                  }
                  if (String.Compare(objField.StringValue, ctl.Text, false) == 0)
                  {
                     return false;
                  }
                  return true;
               }
               if (String.Compare(objField.StringValue, ctl.Text, false) == 0)
               {
                  return false;
               }
               return true;
            }
         }

         return bReturn;
      }

      public Control FirstDirtyControl()
      {
         ControlInformationHolder objField=null;
         Control ctlLowestDirtyControl=null;

        foreach(DictionaryEntry obj in colControlInformation)
        {
            objField = (ControlInformationHolder)obj.Value;
            Control ctl = (Control)obj.Key;

            if(this.IncludeDisabledInvisibleControls || (ctl.Enabled && ctl.Visible))
            {
                if(FieldIsDirty(objField, ctl))
                {
                    if(ctlLowestDirtyControl == null)
                    {
                        ctlLowestDirtyControl = ctl;
                    }
                    else
                    {
                       if(ctlLowestDirtyControl.TabIndex > ctl.TabIndex)
                       {
                          ctlLowestDirtyControl = ctl;
                       }
                    }
                }
             }
        }

        return ctlLowestDirtyControl;
      }

      [DefaultValue(""), Category("NCS Dirty Checker"), Description("Indicates whether this control participates in dirty checking for the DirtyChecker instance")]
      public bool GetDirtyCheckingEnabled(Control ctrl)
      {
         if (this.colControlInformation[ctrl] == null)
         {
            return false;
         }
         ControlInformationHolder obj = (ControlInformationHolder) this.colControlInformation[ctrl];
         return obj.DirtyCheckingEnabled;
      }

      [DebuggerStepThrough]
      private void InitializeComponent()
      {
         this.components = new Container();
      }

      public void KeyUpHandler(object sender, KeyEventArgs e)
      {
         this.CheckForDirtyChange(RuntimeHelpers.GetObjectValue(sender));
      }

      public void MouseUpHandler(object sender, MouseEventArgs e)
      {
         this.CheckForDirtyChange(RuntimeHelpers.GetObjectValue(sender));
      }

      public void RefreshIsDirty()
      {
         this.CheckForDirtyChange(this);
      }

      public void ResumeDirtyChecking()
      {
         this.mbExternalSuspended = false;
         this.RefreshIsDirty();
      }

      private void SetControlType(ControlInformationHolder objProvidedProperties, Control ctl)
      {
         ctl.Click += new EventHandler(this.CommonHandler);
         ctl.MouseUp += new MouseEventHandler(this.MouseUpHandler);
         ctl.KeyUp += new KeyEventHandler(this.KeyUpHandler);
         objProvidedProperties.ControlType = ControlType.typeOther;
         if (ctl is TextBox)
         {
            objProvidedProperties.ControlType = ControlType.typeTextbox;
            TextBox box1 = (TextBox) ctl;
            box1.TextChanged += new EventHandler(this.CommonHandler);
         }
         else if (ctl is RichTextBox)
         {
            objProvidedProperties.ControlType = ControlType.typeRichTextBox;
            RichTextBox box2 = (RichTextBox) ctl;
            box2.TextChanged += new EventHandler(this.CommonHandler);
         }
         else if (ctl is ListBox)
         {
            objProvidedProperties.ControlType = ControlType.typeListBox;
            ListBox box3 = (ListBox) ctl;
            box3.TextChanged += new EventHandler(this.CommonHandler);
            box3.SelectedValueChanged += new EventHandler(this.CommonHandler);
         }
         else if (ctl is ComboBox)
         {
            objProvidedProperties.ControlType = ControlType.typeComboBox;
            ComboBox box4 = (ComboBox) ctl;
            box4.TextChanged += new EventHandler(this.CommonHandler);
            box4.SelectedValueChanged += new EventHandler(this.CommonHandler);
         }
         else if (ctl is RadioButton)
         {
            objProvidedProperties.ControlType = ControlType.typeRadioButton;
            RadioButton button1 = (RadioButton) ctl;
            button1.CheckedChanged += new EventHandler(this.CommonHandler);
         }
         else if (ctl is CheckBox)
         {
            objProvidedProperties.ControlType = ControlType.typeCheckBox;
            CheckBox box5 = (CheckBox) ctl;
            box5.CheckedChanged += new EventHandler(this.CommonHandler);
         }
         else if (ctl is DateTimePicker)
         {
            objProvidedProperties.ControlType = ControlType.typeDatePicker;
            DateTimePicker picker1 = (DateTimePicker) ctl;
            picker1.ValueChanged += new EventHandler(this.CommonHandler);
         }
         else if (ctl is MonthCalendar)
         {
            objProvidedProperties.ControlType = ControlType.typeMonthCalendar;
            MonthCalendar calendar1 = (MonthCalendar) ctl;
            calendar1.DateChanged += new DateRangeEventHandler(this.DateChangedHandler);
         }
         else
         {
            ctl.TextChanged += new EventHandler(this.CommonHandler);
         }
      }

      public void SetDirtyCheckingEnabled(Control ctrl, bool value)
      {
         if (value)
         {
            ControlInformationHolder obj = new ControlInformationHolder();
            if (this.colControlInformation[ctrl] == null)
            {
               obj.DirtyCheckingEnabled = value;
               this.colControlInformation.Add(ctrl, obj);
            }
            else
            {
               obj = (ControlInformationHolder) this.colControlInformation[ctrl];
               if (obj.DirtyCheckingEnabled == value)
               {
                  return;
               }
               obj.DirtyCheckingEnabled = value;
            }
            this.SetControlType(obj, ctrl);
         }
         else if (!this.colControlInformation.Contains(ctrl))
         {
            ControlInformationHolder obj = new ControlInformationHolder();
            obj.DirtyCheckingEnabled = value;
            this.colControlInformation.Add(ctrl, obj);
         }
         else
         {
            ControlInformationHolder obj = new ControlInformationHolder();
            obj = (ControlInformationHolder) this.colControlInformation[ctrl];
            if (obj.DirtyCheckingEnabled)
            {
               ctrl.Click -= new EventHandler(this.CommonHandler);
               ctrl.MouseUp -= new MouseEventHandler(this.MouseUpHandler);
               ctrl.KeyUp -= new KeyEventHandler(this.KeyUpHandler);
               switch (obj.ControlType)
               {
                  case ControlType.typeTextbox:
                  {
                     TextBox box5 = (TextBox) ctrl;
                     box5.TextChanged -= new EventHandler(this.CommonHandler);
                     break;
                  }
                  case ControlType.typeDatePicker:
                  {
                     DateTimePicker picker1 = (DateTimePicker) ctrl;
                     picker1.ValueChanged -= new EventHandler(this.CommonHandler);
                     break;
                  }
                  case ControlType.typeMonthCalendar:
                  {
                     MonthCalendar calendar1 = (MonthCalendar) ctrl;
                     calendar1.DateChanged -= new DateRangeEventHandler(this.DateChangedHandler);
                     break;
                  }
                  case ControlType.typeRadioButton:
                  {
                     RadioButton button1 = (RadioButton) ctrl;
                     button1.CheckedChanged -= new EventHandler(this.CommonHandler);
                     break;
                  }
                  case ControlType.typeCheckBox:
                  {
                     CheckBox box3 = (CheckBox) ctrl;
                     box3.CheckedChanged -= new EventHandler(this.CommonHandler);
                     break;
                  }
                  case ControlType.typeListBox:
                  {
                     ListBox box2 = (ListBox) ctrl;
                     box2.TextChanged -= new EventHandler(this.CommonHandler);
                     box2.SelectedValueChanged -= new EventHandler(this.CommonHandler);
                     break;
                  }
                  case ControlType.typeComboBox:
                  {
                     ComboBox box1 = (ComboBox) ctrl;
                     box1.TextChanged -= new EventHandler(this.CommonHandler);
                     box1.SelectedValueChanged -= new EventHandler(this.CommonHandler);
                     break;
                  }
                  case ControlType.typeRichTextBox:
                  {
                     RichTextBox box4 = (RichTextBox) ctrl;
                     box4.TextChanged -= new EventHandler(this.CommonHandler);
                     break;
                  }
               }
            }
            obj.DirtyCheckingEnabled = value;
         }
      }

      public void Start()
      {
        // The consuming form calls this method when
        // it is ready to start dirty checking. This is
        // typically after the starting values have been
        // filled into the affected controls. From this
        // point onwards, dirty checking begins. If the
        // IsDirty property is called before a Start is 
        // issued, it will always return a False. 

        // The inverse of this method is the Cancel method, 
        // which stops dirty checking.

        mbExternalSuspended = false;

        // Loop through the fields, telling each field
        // to fill up the data record with information from
        // the control.

        ControlInformationHolder objField;
        foreach(DictionaryEntry obj in colControlInformation)
        {
            objField = (ControlInformationHolder)obj.Value;

            switch(objField.ControlType)
            {
               case ControlType.typeCheckBox:
                    CheckBox ctlCheckBox;
                    ctlCheckBox = (CheckBox)obj.Key;
                    objField.BooleanValue = ctlCheckBox.Checked;
                    objField.CheckStateValue = ctlCheckBox.CheckState;
                    break;
               case ControlType.typeComboBox:
                    ComboBox ctlComboBox;
                    ctlComboBox = (ComboBox)obj.Key;

                    // What we want to look at depends on how the 
                    // combo box is being used. If it//s a drop down only, 
                    // then saving the currently selected index works best.
                    // If info can be typed into it, however, we have to save the text.
                    if(ctlComboBox.DropDownStyle == ComboBoxStyle.DropDownList)
                    {
                       objField.IntegerValue = ctlComboBox.SelectedIndex;
                    }
                    else
                    {
                        objField.StringValue = ctlComboBox.Text;
                    }
                  break;
                case ControlType.typeDatePicker:
                    DateTimePicker ctlDateTimePicker;
                    ctlDateTimePicker = (DateTimePicker)obj.Key;
                    objField.BooleanValue = ctlDateTimePicker.Checked;
                    objField.StringValue = ctlDateTimePicker.Value.ToLongDateString();
                  break;
               case ControlType.typeListBox:
                    ListBox ctlListBox;
                    ctlListBox = (ListBox)obj.Key;

                    // If a listbox is multi-select, then have to hang onto
                    // all selected indices. So what we save depends on the
                    // selection mode.
                    if(ctlListBox.SelectionMode == SelectionMode.One)
                    {
                       objField.IntegerValue = ctlListBox.SelectedIndex;
                    }
                    else
                    {
                        // We have a function that stores a string
                        // representation of the indices.
                        string sIndices;
                        sIndices = ConstructIndices(ctlListBox);

                        objField.StringValue = sIndices;
                    }
                  break;
               case ControlType.typeMonthCalendar:

                    MonthCalendar ctlMonthCalendar;
                    ctlMonthCalendar = (MonthCalendar)obj.Key;

                    // This control can have a date range selected, 
                    // so we have to save both start and end point of the range.
                    objField.DateValue = ctlMonthCalendar.SelectionStart;
                    objField.EndDateValue = ctlMonthCalendar.SelectionEnd;
                  break;
               case ControlType.typeOther:
               case ControlType.typeTextbox:
                    Control ctl;
                    ctl = (Control)obj.Key;

                    // This will not work universally for all unknown controls.
                    // However, it should work for many of them.
                    objField.StringValue = ctl.Text;
                  break;
               case ControlType.typeRadioButton:
                    RadioButton ctlRadioButton;
                    ctlRadioButton = (RadioButton)obj.Key;
                    objField.BooleanValue = ctlRadioButton.Checked;
                  break;
               case ControlType.typeRichTextBox:
                    RichTextBox ctlRichTextBox;
                    ctlRichTextBox = (RichTextBox)obj.Key;

                    // The rich text box can also have formatting
                    // changes, and these are exposed via the rtf property.
                    // To speed things up, instead of storing the whole
                    // text (plain and rtf versions), we//ll just store
                    // a hash code.
                       objField.IntegerValue = (ctlRichTextBox.Text + ctlRichTextBox.Rtf).GetHashCode();
                  break;
              }
        }
        m_bDirtyCheckingStarted = true;
        CheckForDirtyChange(this);
      }

      public void SuspendDirtyChecking()
      {
         this.mbExternalSuspended = true;
      }


      // Properties
      public static iCustomIsDirtyContainer CustomIsDirtyContainer
      {
         get{return DirtyChecker.m_objCustomIsDirty;}
         set{DirtyChecker.m_objCustomIsDirty = value;}
      }

      [Browsable(false)]
      public bool ExternalDirty
      {
         set
         {
            this.m_blnExternalDirty = value;
            this.CheckForDirtyChange(this);
         }
      }

      [Description("Indicates whether the Dirty Checker includes disabled and invisible controls when checking dirty state"), DefaultValue(typeof(bool), "True")]
      public bool IncludeDisabledInvisibleControls
      {
         get
         {
            return this.mbIncludeDisabledInvisibleControls;
         }
         set
         {
            this.mbIncludeDisabledInvisibleControls = value;
         }
      }

      [Browsable(false)]
      public bool IsDirty
      {
         get
         {
            if (this.m_bDirtyCheckingStarted)
            {
               ControlInformationHolder objField = null;

               // Loop through the controls that are enabled for dirty checking.
               foreach(DictionaryEntry obj in colControlInformation)
               {
                  objField = (ControlInformationHolder)obj.Value;
                  Control ctl = (Control)obj.Key;

                  // Depending on a property setting, we might be skipping invisible or disabled controls. 
                  if(this.IncludeDisabledInvisibleControls || (ctl.Enabled && ctl.Visible))
                  {
                     if(FieldIsDirty(objField, ctl))
                     {
                        return true;
                     }
                  }
               }
            }
            return false;
         }
      }

      [Browsable(false)]
      public bool LastDirty
      {
         get{return this.mbLastDirty;}
         set{this.mbLastDirty = value;}
      }


      // Fields
      private Hashtable colControlInformation;
      private IContainer components;
      private bool m_bDirtyCheckingStarted;
      private bool m_blnExternalDirty;
      private bool m_bSuspendDirtyChecking;
      private static iCustomIsDirtyContainer m_objCustomIsDirty;
      private bool mbExternalSuspended;
      private bool mbIncludeDisabledInvisibleControls;
      private bool mbLastDirty;

      // Nested Types
      private class ControlInformationHolder
      {
         // Methods
         public ControlInformationHolder()
         {
         }


         // Properties
         public bool DirtyCheckingEnabled
         {
            get{return this.mbDirtyCheckingEnabled;}
            set{this.mbDirtyCheckingEnabled = value;}
         }


         // Fields
         public bool BooleanValue;
         public CheckState CheckStateValue;
         public DirtyChecker.ControlType ControlType;
         public DateTime DateValue;
         public DateTime EndDateValue;
         public int IntegerValue;
         private bool mbDirtyCheckingEnabled;
         public string StringValue;
      }

      public enum ControlType
      {
         typeTextbox,
         typeDatePicker,
         typeMonthCalendar,
         typeRadioButton,
         typeCheckBox,
         typeListBox,
         typeComboBox,
         typeRichTextBox,
         typeOther
      }

      public delegate void DirtyChangedEventHandler(object sender, DirtyChangedEventArgs e);

   }

   public interface iCustomIsDirtyContainer
   {
      // Methods
      bool ControlSupported(Control ctrl);
      bool CustomIsDirty(Control ctrl);
   }

   public class DirtyChangedEventArgs : EventArgs
   {
      // Fields
      private bool m_blnDirty;

      // Methods
      public DirtyChangedEventArgs()
      {
      }

      // Properties
      public bool Dirty
      {
         get{return this.m_blnDirty;}
         set{this.m_blnDirty = value;}
      }
   }
}

