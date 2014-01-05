using System;
using System.Linq;
using System.Collections.Generic;

using Windows.Storage;
using Windows.UI.Xaml.Controls;

using XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations;
using XConductor.Domain.Seedwork.Abstractions.Settings;

namespace XConductor.Domain.W8.Playback.Settings
{
    internal class AudioPlayerSettings : ValidatableObject, ISettings
    {
        [Requared("InputFile")]
        public StorageFile InputFile { get; set; }

        [Requared("MediaElement")]
        public MediaElement MediaElement { get; set; }
    }
}
