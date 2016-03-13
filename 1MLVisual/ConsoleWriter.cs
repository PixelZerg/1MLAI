﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1MLVisual
{
    public class ListBoxWriter : TextWriter //this class redirects console.writeline to debug listbox
    {
        private readonly ListBox _list;
        private StringBuilder _content = new StringBuilder();

        public ListBoxWriter(ListBox list)
        {
            _list = list;
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
        public override void Write(char value)
        {
            base.Write(value);
            _content.Append(value);

            if (value != '\n') return;
            if (_list.InvokeRequired)
            {
                try
                {
                    _list.Invoke(new MethodInvoker(() => _list.Items.Add(_content.ToString())));
                    _list.Invoke(new MethodInvoker(() => _list.SelectedIndex = _list.Items.Count - 1));
                    _list.Invoke(new MethodInvoker(() => _list.SelectedIndex = -1));
                }
                catch (ObjectDisposedException ex)
                {
                   // Console.WriteLine(Resources.Exception_raised + " (" + ex.Message + "): " + ex);
                }
            }
            else
            {
                _list.Items.Add(_content.ToString());
                _list.SelectedIndex = _list.Items.Count - 1;
                _list.SelectedIndex = -1;
            }
            _content = new StringBuilder();
        }
    }
}
