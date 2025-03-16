using KlonsLIB.Forms;
using NHunspell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleLoacalAIChat
{
    public class RTBSpellChecker : IDisposable
    {
        RichTextBox RTB;
        Form Form;
        ContextMenuStrip SuggestionsMenu = null;

        Hunspell hunspellEN = null;
        List<string> words = new List<string>();
        List<int> wordsstartindex = new List<int>();
        Stopwatch stopwatch = new Stopwatch();
        long lastkeyprestime = 0;
        bool texthasredwords = false;
        Dictionary<string, bool> WordsTested = new Dictionary<string, bool>();
        bool disposedValue;
        int testDalay = 3000;
        System.Windows.Forms.Timer timer = null;

        public RTBSpellChecker(RichTextBox rtb)
        {
            RTB = rtb;
            Form = RTB.FindForm();
            hunspellEN = new Hunspell("en_US.aff", "en_US.dic");
            stopwatch.Start();
            RTB.KeyDown += rtb_KeyDown;
            RTB.MouseUp += rtb_MouseUp;
            timer = new System.Windows.Forms.Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = testDalay;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (lastkeyprestime == 0) return;
            timer.Stop();
            if (RTB.Disposing || RTB.IsDisposed) return;
            DoAfterTextChanged();
            lastkeyprestime = 0;
            timer.Enabled = true;
        }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    stopwatch.Stop();
                    timer.Stop();
                    RTB.KeyDown -= rtb_KeyDown;
                    RTB.MouseUp -= rtb_MouseUp;
                    SuggestionsMenu?.Dispose();
                    if (hunspellEN != null) hunspellEN.Dispose();
                    hunspellEN = null;
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        private void rtb_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            RTB.SelectionLength = 0;
            RTB.SelectionStart = RTB.GetCharIndexFromPosition(e.Location);
            int k = FindWord(RTB.SelectionStart);
            if (k == -1) return;
            var word = words[k];
            ShowSuggestions(word, e.Location);
        }

        private void rtb_KeyDown(object sender, KeyEventArgs e)
        {
            lastkeyprestime = stopwatch.ElapsedMilliseconds;
            timer.Stop();
            timer.Enabled = true;
        }

        private void SuggestionsMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var word = e.ClickedItem.Text;
            ReplaceCurrentWordWith(word);
            DoAfterTextChanged();
        }

        void ShowSuggestions(string word, Point pt)
        {
            var sgs = GetSuggestions(word);
            if (sgs.Count == 0) return;
            if (SuggestionsMenu == null)
            {
                SuggestionsMenu = new ContextMenuStrip();
                SuggestionsMenu.Font = Form.Font;
                SuggestionsMenu.BackColor = Form.BackColor;
                SuggestionsMenu.ForeColor = Form.ForeColor;
                SuggestionsMenu.Renderer = ColorThemeHelper.MyToolStripRenderer;
                ColorThemeHelper.ApplyToControlA(SuggestionsMenu, MyData.Settings.ColorTheme);
                SuggestionsMenu.ShowCheckMargin = false;
                SuggestionsMenu.ShowImageMargin = false;
                SuggestionsMenu.ItemClicked += SuggestionsMenu_ItemClicked;
            }
            SuggestionsMenu.Items.Clear();
            for (int i = 0; i < sgs.Count; i++)
            {
                var s = sgs[i];
                var tsmi = new ToolStripMenuItem(s);
                SuggestionsMenu.Items.Add(tsmi);
            }
            SuggestionsMenu.Show(RTB, pt);
        }

        void DoAfterTextChanged()
        {
            lastkeyprestime = stopwatch.ElapsedMilliseconds;

            var text = RTB.Text;
            if (string.IsNullOrEmpty(text)) return;

            int curpos = RTB.SelectionStart;
            int cursel = RTB.SelectionLength;

            if (texthasredwords)
            {
                RTB.SelectionStart = 0;
                RTB.SelectionLength = text.Length;
                RTB.SelectionColor = RTB.ForeColor;
            }

            texthasredwords = false;
            words = SplitWords(text);
            wordsstartindex = GetPositions(text, words);
            for (int i = 0; i < words.Count; i++)
            {
                var word = words[i];
                var isok = IsGoodWord(word);
                if (isok) continue;
                RTB.SelectionLength = 0;
                RTB.SelectionStart = wordsstartindex[i];
                RTB.SelectionLength = word.Length;
                RTB.SelectionColor = Color.IndianRed;
                texthasredwords = true;
            }

            RTB.SelectionStart = curpos;
            RTB.SelectionLength = cursel;

            lastkeyprestime = 0;
        }

        public void ReplaceCurrentWordWith(string word)
        {
            int curpos = RTB.SelectionStart;
            int k = FindWord(curpos);
            if (k == -1) return;
            int k1 = wordsstartindex[k];
            int len = words[k].Length;
            RTB.SelectionStart = k1;
            RTB.SelectionLength = len;
            RTB.SelectedText = word;

            RTB.SelectionLength = 0;
            RTB.SelectionStart = curpos;
        }

        public List<string> SplitWords(string text)
        {
            string pattern = @"\b\w+\b";
            MatchCollection matches = Regex.Matches(text, pattern);
            var ret = matches.Select(x => x.Value).ToList();
            return ret;
        }

        public List<int> GetPositions(string text, List<string> words)
        {
            var ret = new List<int>();
            int pos = 0;
            foreach (string word in words)
            {
                var p1 = text.IndexOf(word, pos);
                if (p1 == -1)
                    continue;
                ret.Add(p1);
                pos += word.Length;
            }
            return ret;
        }

        int FindWord(int pos)
        {
            if (words.Count == 0) return -1;
            if (wordsstartindex.Count == 0) return -1;
            if (wordsstartindex.Count != words.Count) return -1;
            for (int i = 0; i < words.Count; i++)
            {
                int k1 = wordsstartindex[i];
                int k2 = k1 + words[i].Length;
                if (pos < k1) break;
                if (k1 <= pos && pos <= k2)
                    return i;
            }
            return -1;
        }

        List<string> GetSuggestions(string word)
        {
            var ret = hunspellEN.Suggest(word);
            if (ret == null) return [];
            ret = ret.Take(8).ToList();
            return ret;
        }

        bool IsGoodWord(string word)
        {
            if (WordsTested.TryGetValue(word, out var result)) return result;
            var isok = hunspellEN.Spell(word);
            WordsTested[word] = isok;
            return isok;
        }

    }
}
