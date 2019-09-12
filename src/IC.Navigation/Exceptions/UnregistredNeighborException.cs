﻿using IC.Navigation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.Exceptions
{
    public class UnregistredNeighborException : Exception
    {
        public UnregistredNeighborException()
        {
        }

        public UnregistredNeighborException(INavigable neighbor, Type declaringClass)
            : base($"\"{neighbor.GetType().FullName}\" is not registred in \"{declaringClass.GetType().FullName}\".")
        {
        }
    }
}