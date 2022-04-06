using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace PixelAimbot.Classes.Misc
{

    public class CustomTabs : TabControl
    {

        private readonly StringFormat CenterSringFormat = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center
        };


        private Color activeColor = Color.FromArgb(0, 122, 204);
        private Color backTabColor = Color.FromArgb(28, 28, 28);
        private Color borderColor = Color.FromArgb(30, 30, 30);
        private Color closingButtonColor = Color.WhiteSmoke;
        private string closingMessage;
        private Color headerColor = Color.FromArgb(45, 45, 48);
        private Color horizLineColor = Color.FromArgb(0, 122, 204);
        private TabPage predraggedTab;
        private Color textColor = Color.FromArgb(255, 255, 255);
        public bool ShowClosingButton { get; set; }
        public Color selectedTextColor = Color.FromArgb(255, 255, 255);
        public CustomTabs()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw
                | ControlStyles.OptimizedDoubleBuffer,
                true);
            DoubleBuffered = true;
            SizeMode = TabSizeMode.Normal;
            ItemSize = new Size(220, 16);
            AllowDrop = true;
        }

        [Category("Colors"), Browsable(true), Description("The color of the selected page")]
        public Color ActiveColor
        {
            get
            {
                return this.activeColor;
            }

            set
            {
                this.activeColor = value;
            }
        }

        [Category("Colors"), Browsable(true), Description("The color of the background of the tab")]
        public Color BackTabColor
        {
            get
            {
                return this.backTabColor;
            }

            set
            {
                this.backTabColor = value;
            }
        }

        [Category("Colors"), Browsable(true), Description("The color of the border of the control")]
        public Color BorderColor
        {
            get
            {
                return this.borderColor;
            }

            set
            {
                this.borderColor = value;
            }
        }

        [Category("Colors"), Browsable(true), Description("The color of the closing button")]
        public Color ClosingButtonColor
        {
            get
            {
                return this.closingButtonColor;
            }

            set
            {
                this.closingButtonColor = value;
            }
        }

        [Category("Options"), Browsable(true), Description("The message that will be shown before closing.")]
        public string ClosingMessage
        {
            get
            {
                return this.closingMessage;
            }

            set
            {
                this.closingMessage = value;
            }
        }

        [Category("Colors"), Browsable(true), Description("The color of the header.")]
        public Color HeaderColor
        {
            get
            {
                return this.headerColor;
            }

            set
            {
                this.headerColor = value;
            }
        }

        [Category("Colors"), Browsable(true),
         Description("The color of the horizontal line which is located under the headers of the pages.")]
        public Color HorizontalLineColor
        {
            get
            {
                return this.horizLineColor;
            }

            set
            {
                this.horizLineColor = value;
            }
        }
        [Category("Options"), Browsable(true), Description("Show a Yes/No message before closing?")]
        public bool ShowClosingMessage { get; set; }

        [Category("Colors"), Browsable(true), Description("The color of the title of the page")]
        public Color SelectedTextColor
        {
            get
            {
                return this.selectedTextColor;
            }

            set
            {
                this.selectedTextColor = value;
            }
        }

        [Category("Colors"), Browsable(true), Description("The color of the title of the page")]
        public Color TextColor
        {
            get
            {
                return this.textColor;
            }

            set
            {
                this.textColor = value;
            }
        }


        protected override void CreateHandle()
        {
            base.CreateHandle();
            Alignment = TabAlignment.Top;
        }
        protected override void OnDragOver(DragEventArgs drgevent)
        {
            var draggedTab = (TabPage)drgevent.Data.GetData(typeof(TabPage));
            var pointedTab = getPointedTab();

            if (ReferenceEquals(draggedTab, predraggedTab) && pointedTab != null)
            {
                drgevent.Effect = DragDropEffects.Move;

                if (!ReferenceEquals(pointedTab, draggedTab))
                {
                    this.ReplaceTabPages(draggedTab, pointedTab);
                }
            }

            base.OnDragOver(drgevent);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            predraggedTab = getPointedTab();
            var p = e.Location;
            if (!this.ShowClosingButton)
            {
            }
            else
            {
                for (var i = 0; i < this.TabCount; i++)
                {
                    var r = this.GetTabRect(i);
                    r.Offset(r.Width - 15, 2);
                    r.Width = 10;
                    r.Height = 10;
                    if (!r.Contains(p))
                    {
                        continue;
                    }

                    if (this.ShowClosingMessage)
                    {
                        if (DialogResult.Yes == MessageBox.Show(this.ClosingMessage, "Close", MessageBoxButtons.YesNo))
                        {
                            this.TabPages.RemoveAt(i);
                        }
                    }
                    else
                    {
                        this.TabPages.RemoveAt(i);
                    }
                }
            }

            base.OnMouseDown(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && predraggedTab != null)
            {
                this.DoDragDrop(predraggedTab, DragDropEffects.Move);
            }

            base.OnMouseMove(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            predraggedTab = null;
            base.OnMouseUp(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var Drawer = g;

            Drawer.SmoothingMode = SmoothingMode.HighQuality;
            Drawer.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Drawer.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            Drawer.Clear(this.headerColor);
            try
            {
                SelectedTab.BackColor = this.backTabColor;
            }
            catch
            {
                // ignored
            }

            try
            {
                SelectedTab.BorderStyle = BorderStyle.None;
            }
            catch
            {
                // ignored
            }

            for (var i = 0; i <= TabCount - 1; i++)
            {
                var Header = new Rectangle(
                    new Point(GetTabRect(i).Location.X + 2, GetTabRect(i).Location.Y),
                    new Size(GetTabRect(i).Width, GetTabRect(i).Height));
                var HeaderSize = new Rectangle(Header.Location, new Size(Header.Width, Header.Height));
                Brush ClosingColorBrush = new SolidBrush(this.closingButtonColor);

                if (i == SelectedIndex)
                {
                    Drawer.FillRectangle(new SolidBrush(this.headerColor), HeaderSize);

                    Drawer.FillRectangle(
                        new SolidBrush(this.activeColor),
                        new Rectangle(Header.X - 5, Header.Y - 3, Header.Width, Header.Height + 5));

                    Drawer.DrawString(
                        TabPages[i].Text,
                        Font,
                        new SolidBrush(this.selectedTextColor),
                        HeaderSize,
                        this.CenterSringFormat);

                    if (this.ShowClosingButton)
                    {
                        e.Graphics.DrawString("X", Font, ClosingColorBrush, HeaderSize.Right - 17, 3);
                    }
                }
                else
                {
                    Drawer.DrawString(
                        TabPages[i].Text,
                        Font,
                        new SolidBrush(this.textColor),
                        HeaderSize,
                        this.CenterSringFormat);
                }
            }

            // Draws the horizontal line
            Drawer.DrawLine(new Pen(this.horizLineColor, 5), new Point(0, 19), new Point(Width, 19));

            // Draws the background of the tab control
            Drawer.FillRectangle(new SolidBrush(this.backTabColor), new Rectangle(0, 20, Width, Height - 20));

            // Draws the border of the TabControl
            Drawer.DrawRectangle(new Pen(this.borderColor, 2), new Rectangle(0, 0, Width, Height));
            Drawer.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }

        private TabPage getPointedTab()
        {
            for (var i = 0; i <= this.TabPages.Count - 1; i++)
            {
                if (this.GetTabRect(i).Contains(this.PointToClient(Cursor.Position)))
                {
                    return this.TabPages[i];
                }
            }

            return null;
        }
        private void ReplaceTabPages(TabPage Source, TabPage Destination)
        {
            var SourceIndex = this.TabPages.IndexOf(Source);
            var DestinationIndex = this.TabPages.IndexOf(Destination);

            this.TabPages[DestinationIndex] = Source;
            this.TabPages[SourceIndex] = Destination;

            if (this.SelectedIndex == SourceIndex)
            {
                this.SelectedIndex = DestinationIndex;
            }
            else if (this.SelectedIndex == DestinationIndex)
            {
                this.SelectedIndex = SourceIndex;
            }

            this.Refresh();
        }


    }
}
