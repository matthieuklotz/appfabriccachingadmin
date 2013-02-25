// -----------------------------------------------------------------------
// <copyright file="CacheServiceStatusToImageConverter.cs" company="Matthieu Klotz">
// Copyright (c) 2013, Matthieu Klotz
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of Matthieu Klotz nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// -----------------------------------------------------------------------

namespace AppFabric.Admin.Clients.Desktop.Converters
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;
    using System.Windows.Media.Imaging;
    using AppFabric.Admin.Clients.Common.Data;

    public class CacheServiceStatusToImageConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Instance of the converter.
        /// </summary>
        private static readonly CacheServiceStatusToImageConverter Instance = new CacheServiceStatusToImageConverter();

        /// <summary>
        /// Cached bitmapimages.
        /// </summary>
        private static readonly ConcurrentDictionary<CacheServiceStatus, BitmapImage> Images = new ConcurrentDictionary<CacheServiceStatus, BitmapImage>();

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage image = null;
            if (value != null)
            {
                CacheServiceStatus status = (CacheServiceStatus)value;
                image = Images.GetOrAdd(status, GetImageStatus);
            }

            return image;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

        /// <summary>
        /// Gets the image status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>The image that correspond to the status.</returns>
        private static BitmapImage GetImageStatus(CacheServiceStatus status)
        {
            string statusimage = string.Format(CultureInfo.InvariantCulture, "pack://application:,,,/Skins/status_{0}.png", status);
            BitmapImage image = new BitmapImage(new Uri(statusimage));
            image.CacheOption = BitmapCacheOption.OnLoad;
            return image;
        }
    }
}
