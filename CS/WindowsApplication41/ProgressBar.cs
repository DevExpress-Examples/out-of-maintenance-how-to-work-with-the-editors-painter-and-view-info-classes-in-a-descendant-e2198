using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.XtraEditors.Registrator;
using DevExpress.Utils.Drawing;
using DevExpress.Utils;
[UserRepositoryItem("RegisterMyProgressBar")]
public class MyRepositoryItemProgressBar : RepositoryItem
{
        Color _barColor;
    public Color BarColor
    {
        get { return _barColor; }
        set
        {
            if (_barColor == value) return;
            _barColor = value;
            this.OnPropertiesChanged();
        }
    }
    static MyRepositoryItemProgressBar() { RegisterMyProgressBar(); }
    public static void RegisterMyProgressBar()
    {
        EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo("MyProgressBarControl", typeof(MyProgressBarControl), typeof(MyRepositoryItemProgressBar), typeof(MyProgressBarViewInfo), new MyProgressBarPainter(), true, null, typeof(DevExpress.Accessibility.PopupEditAccessible)));
    }

    public new MyRepositoryItemProgressBar Properties { get { return this; } }
    private static object positionChanged = new object();
    [Browsable(false)]
    public override string EditorTypeName { get { return "MyProgressBarControl"; } }
    int _minimum, _maximum;
    public MyRepositoryItemProgressBar()
    {
		
		this.fAutoHeight = false;
        this._barColor = Color.YellowGreen;
        this._minimum = 0;
        this._maximum = 100;
    }
    public override void Assign(RepositoryItem item)
    {
        MyRepositoryItemProgressBar source = item as MyRepositoryItemProgressBar;
        BeginUpdate();
        try
        {
            base.Assign(item);
            if (source == null) return;
            this._maximum = source.Maximum;
            this._minimum = source.Minimum;
            this.BarColor = source.BarColor;
        }
        finally
        {
            EndUpdate();
        }
        Events.AddHandler(positionChanged, source.Events[positionChanged]);
    }
    protected new MyProgressBarControl OwnerEdit { get { return base.OwnerEdit as MyProgressBarControl; } }
    [Category(CategoryName.Behavior), Description("Gets or sets the control's minimum value."), RefreshProperties(RefreshProperties.All), DefaultValue(0)]
    public int Minimum
    {
        get { return _minimum; }
        set
        {
            if (!IsLoading)
            {
                if (value > Maximum) value = Maximum;
            }
            if (Minimum == value) return;
            _minimum = value;
            OnMinMaxChanged();
            OnPropertiesChanged();
        }
    }
    [Category(CategoryName.Behavior), Description("Gets or sets the control's maximum value."), RefreshProperties(RefreshProperties.All), DefaultValue(100)]
    public int Maximum
    {
        get { return _maximum; }
        set
        {
            if (!IsLoading)
            {
                if (value < Minimum) value = Minimum;
            }
            if (Maximum == value) return;
            _maximum = value;
            OnMinMaxChanged();
            OnPropertiesChanged();
        }
    }
    protected internal virtual int ConvertValue(object val)
    {
        try
        {
            return CheckValue(Convert.ToInt32(val));
        }
        catch { }
        return Minimum;
    }
    protected internal virtual int CheckValue(int val)
    {
        if (IsLoading) return val;
        val = Math.Max(val, Minimum);
        val = Math.Min(val, Maximum);
        return val;
    }
    public override VisualBrick GetBrick(PrintCellHelperInfo info)
    {
        ProgressBarBrick brick = (ProgressBarBrick)base.GetBrick(info);
        brick.Position = ConvertValue(info.EditValue);
        return brick;
    }
    protected virtual void OnMinMaxChanged()
    {
        if (OwnerEdit != null) OwnerEdit.OnMinMaxChanged();
    }
    [Description("Occurs after the value of the ProgressBarControl.Position property has been changed."), Category(CategoryName.Events)]
    public event EventHandler PositionChanged
    {
        add { this.Events.AddHandler(positionChanged, value); }
        remove { this.Events.RemoveHandler(positionChanged, value); }
    }
    protected override void RaiseEditValueChangedCore(EventArgs e)
    {
        base.RaiseEditValueChangedCore(e);
        RaisePositionChanged(e);
    }
    protected internal virtual void RaisePositionChanged(EventArgs e)
    {
        EventHandler handler = (EventHandler)this.Events[positionChanged];
        if (handler != null) handler(GetEventSender(), e);
    }
}
 [ToolboxItem(true)]
public class MyProgressBarControl : BaseEdit 
{

    static MyProgressBarControl()
    {
        MyRepositoryItemProgressBar.RegisterMyProgressBar();
    }
     public MyProgressBarControl() {
			this.TabStop = false;
			this.fOldEditValue = this.fEditValue = 0;
		}
    [Browsable(false)]
    public override string EditorTypeName { get { return "MyProgressBarControl"; } }
    [Description("Gets an object containing properties, methods and events specific to progress bar controls."), Category(CategoryName.Properties), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public new MyRepositoryItemProgressBar Properties { get { return base.Properties as MyRepositoryItemProgressBar; } }
    [Browsable(false), DefaultValue(0)]
    public override object EditValue
    {
        get { return base.EditValue; }
        set { base.EditValue = ConvertCheckValue(value); }
    }
    [Category(CategoryName.Appearance), Description("Gets or sets progress bar position."), RefreshProperties(RefreshProperties.All), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(0),
    Bindable(ControlConstants.NonObjectBindable)]
    public virtual int Position
    {
        get { return Properties.ConvertValue(EditValue); }
        set { EditValue = Properties.CheckValue(value); }
    }
    protected virtual object ConvertCheckValue(object val)
    {
        if (val == null || val is DBNull) return val;
        int converted = Properties.ConvertValue(val);
        try
        {
            if (converted == Convert.ToInt32(val))
                return val;
        }
        catch { }
        return converted;
    }
    protected internal virtual void OnMinMaxChanged()
    {
        if (IsLoading) return;
        Position = Properties.CheckValue(Position);
    }
    protected internal virtual void UpdatePercent()
    {
        MyProgressBarViewInfo vi = ViewInfo as MyProgressBarViewInfo;
        if (vi == null) return;
        vi.UpdateProgressInfo(vi.ProgressInfo);
    }
    public event EventHandler PositionChanged
    {
        add { Properties.PositionChanged += value; }
        remove { Properties.PositionChanged -= value; }
    }
}

public class MyObjectInfoArgs : ObjectInfoArgs {
    public MyObjectInfoArgs() { }
    public Color BackgroundColor;
    public Color BarColor;
    public float Percent = 0;
}

[ToolboxBitmap(typeof(MyProgressBarControl), "")]
	public class MyProgressBarViewInfo : BaseEditViewInfo {
        int position;
        float percents;
        MyObjectInfoArgs progressInfo;
    		public MyProgressBarViewInfo(RepositoryItem item) : base(item) {
            
		}
		protected override void Assign(BaseControlViewInfo info) {
			base.Assign(info);
			MyProgressBarViewInfo be = info as MyProgressBarViewInfo;
			if(be == null) return;
            this.percents = be.percents;
            this.position = be.position;
		}
        protected override void OnEditValueChanged()
        {
            this.position = Item.ConvertValue(EditValue);
            this.percents = 1;
            if (Item.Maximum != Item.Minimum)
            {
                this.percents = Math.Abs((float)(Position - Item.Minimum) / (float)(Item.Maximum - Item.Minimum));
            }
        }
        public virtual int Position { get { return position; } }
        public virtual float Percents { get { return percents; } }
		protected override void ReCalcViewInfoCore(Graphics g, MouseButtons buttons, Point mousePosition, Rectangle bounds) {
			base.ReCalcViewInfoCore(g, buttons, mousePosition, bounds);
			UpdateProgressInfo(ProgressInfo);
		}
		public override void Reset() {
			base.Reset();
			this.progressInfo = CreateInfoArgs();
            this.position = 0;
            this.percents = 0;
		}
        public virtual MyObjectInfoArgs ProgressInfo { get { return progressInfo; } }
		public new MyRepositoryItemProgressBar Item { get { return base.Item as MyRepositoryItemProgressBar; } }
		protected internal virtual void UpdateProgressInfo(MyObjectInfoArgs info) {
			info.Bounds = ContentRect;
            info.BarColor = Item.BarColor;
            info.BackgroundColor = Item.Appearance.BackColor;
            info.Percent = this.Percents;
		}
        protected override ObjectInfoArgs GetBorderArgs(Rectangle bounds)
        {
            MyObjectInfoArgs pi = new MyObjectInfoArgs();
            pi.Bounds = bounds;
            return pi;
        }
        protected override BorderPainter GetBorderPainterCore() {
			BorderPainter bp = base.GetBorderPainterCore();
			if(bp is WindowsXPTextBorderPainter) bp = new WindowsXPProgressBarBorderPainter();
			return bp;
		}
        public override void Offset(int x, int y)
        {
            base.Offset(x, y);
            ProgressInfo.OffsetContent(x, y);
        }
		protected override void CalcContentRect(Rectangle bounds) {
			base.CalcContentRect(bounds);
			UpdateProgressInfo(ProgressInfo);
		}
        protected virtual MyObjectInfoArgs CreateInfoArgs()
        {
			return new MyObjectInfoArgs();
		}
        public override object EditValue
        {
            get { return base.EditValue; }
            set
            {
                this.fEditValue = value;
                OnEditValueChanged();
            }
        }
	}

    public class MyProgressBarPainter : BaseEditPainter
        {
        protected override void DrawContent(ControlGraphicsInfoArgs info)
        {
            MyProgressBarViewInfo vi = info.ViewInfo as MyProgressBarViewInfo;
            info.Graphics.FillRectangle(new SolidBrush(info.ViewInfo.PaintAppearance.BackColor), info.Bounds);
            DrawObject(info.Cache, vi.ProgressInfo);
        }
        public  void DrawObject(GraphicsCache cache, ObjectInfoArgs e)
        {
            if (e == null) return;
            MyObjectInfoArgs ee = e as MyObjectInfoArgs;
            GraphicsCache prev = e.Cache;
            e.Cache = cache;
            try
            {
                e.Cache.Paint.FillRectangle(e.Graphics, new SolidBrush(ee.BackgroundColor), e.Bounds);
                DrawBar(ee);
            }
            finally
            {
                e.Cache = prev;
            }

        }
        protected virtual void DrawBar(MyObjectInfoArgs e)
        {
            
            Brush brush = e.Cache.GetSolidBrush(e.BarColor);
            e.Cache.Paint.FillGradientRectangle(e.Graphics, brush, CalcProgressBounds(e));
        }
        protected virtual Rectangle CalcProgressBounds(MyObjectInfoArgs e)
        {
            Rectangle r = e.Bounds;
            if (e.Percent < 1)
            {
                Size size = r.Size;
                    r.Width = Math.Min(Convert.ToInt32(e.Percent * (float)(size.Width)), size.Width);
            }
            return r;
        }
}

