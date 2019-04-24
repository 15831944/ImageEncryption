using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEncryption_WPF
{
    class Model
    {
        private static volatile Model instance;
        private static readonly object obj = new object();
        public static Model Instance
        {
            get
            {
                if (null == instance)
                {
                    lock (obj)
                    {
                        if (null == instance)
                        {
                            instance = new Model();
                        }
                    }

                }
                return instance;
            }
        }

        public Bitmap Bitmap { get; set; } = null;
    }
}
