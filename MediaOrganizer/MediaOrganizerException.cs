﻿//
// MediaOrganizerException.cs: Custom exception class.
//
// Copyright (C) 2014 Rikard Johansson
//
// This program is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// this program. If not, see http://www.gnu.org/licenses/.
//

using ExifOrganizer.Common;
using System;
using System.Runtime.Serialization;

namespace ExifOrganizer.Organizer
{
    [Serializable]
    public class MediaOrganizerException : CommonException
    {
        public MediaOrganizerException(string message, params object[] args)
            : base(message, args)
        {
        }

        public MediaOrganizerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MediaOrganizerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}