﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ecliptic
{
    public interface IToast
    {
        // отображение всплывающего уведомления
        void Show(string message);
    }
}
