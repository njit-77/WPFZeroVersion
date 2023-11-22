using System.Windows;
using System.Windows.Documents;

namespace WPF.Zero.UserControls
{
    public class BindableRichTextBox : System.Windows.Controls.RichTextBox
    {
        public BindableRichTextBox()
        {
            IsReadOnly = true;
            FontFamily = new System.Windows.Media.FontFamily("Cascadia Code");
            FontSize = 23;
            VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
            HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;

            TextChanged += (s, e) =>
            {
                (s as System.Windows.Controls.RichTextBox).ScrollToEnd();
            };
        }


        #region DependencyProperty

        public static readonly DependencyProperty ParagraphProperty =
            DependencyProperty.Register("Paragraph",
                typeof(Paragraph),
                typeof(BindableRichTextBox),
                new PropertyMetadata(null, new PropertyChangedCallback((d, e) =>
                {
                    if (d is System.Windows.Controls.RichTextBox rtb)
                    {
                        rtb.Document.Blocks.Clear();
                        rtb.Document.Blocks.Add(e.NewValue as Paragraph);
                    }
                })));

        public Paragraph Paragraph
        {
            get { return (Paragraph)GetValue(ParagraphProperty); }
            set { SetValue(ParagraphProperty, value); }
        }

        #endregion


    }
}
