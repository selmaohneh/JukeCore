using System;

namespace JukeCore
{
    public class ChangedProperty
    {
        public string PropertyName { get; }

        public string PropertyValue { get; }

        public ChangedProperty(string propertyName, string propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }
    }

    /// <summary>
    /// Data model of the juke core state
    /// </summary>
    public class JukeCoreDataModel
    {
        private EPlaybackState _playbackState = EPlaybackState.Stopped;

        /// <summary>
        /// current playback state
        /// </summary>
        public EPlaybackState PlaybackState
        {
            get => _playbackState;
            set
            {
                if (_playbackState != value)
                {
                    
                    _playbackState = value;
                    OnPropertyChange(new ChangedProperty(nameof(PlaybackState), value.ToString()));
                }
            }
        }

        private long _mediaDurationMs;

        /// <summary>
        /// duration of current played track in ms
        /// </summary>
        public long MediaDurationMs
        {
            get => _mediaDurationMs;
            set
            {
                if (_mediaDurationMs != value)
                {

                    _mediaDurationMs = value;
                    OnPropertyChange(new ChangedProperty(nameof(MediaDurationMs), value.ToString()));
                }
            }
        }

        private long _mediaPositionMs;

        /// <summary>
        /// position in current played track in ms
        /// </summary>
        public long MediaPositionMs
        {
            get => _mediaPositionMs;
            set
            {
                if (_mediaPositionMs != value)
                {

                    _mediaPositionMs = value;
                    OnPropertyChange(new ChangedProperty(nameof(MediaPositionMs), value.ToString()));
                }
            }
        }

        private byte _volumePercent;

        /// <summary>
        /// playback volume in percent
        /// </summary>
        public byte VolumePercent
        {
            get => _volumePercent;
            set
            {
                if (_volumePercent != value)
                {

                    _volumePercent = value;
                    OnPropertyChange(new ChangedProperty(nameof(VolumePercent), value.ToString()));
                }
            }
        }

        private string _mediaFilename;

        /// <summary>
        /// filename of current played track in ms
        /// </summary>
        public string MediaFilename
        {
            get => _mediaFilename;
            set
            {
                if (_mediaFilename != value)
                {

                    _mediaFilename = value;
                    OnPropertyChange(new ChangedProperty(nameof(MediaFilename), value));
                }
            }
        }

        private EButtonState _volumeDownButtonState;

        /// <summary>
        /// State of the volume down button
        /// </summary>
        public EButtonState VolumeDownButtonState
        {
            get => _volumeDownButtonState;
            set
            {
                if (_volumeDownButtonState != value)
                {

                    _volumeDownButtonState = value;
                    OnPropertyChange(new ChangedProperty(nameof(VolumeDownButtonState), value.ToString()));
                }
            }
        }

        private EButtonState _volumeUpButtonState;

        /// <summary>
        /// State of the volume up button
        /// </summary>
        public EButtonState VolumeUpButtonState
        {
            get => _volumeUpButtonState;
            set
            {
                if (_volumeUpButtonState != value)
                {

                    _volumeUpButtonState = value;
                    OnPropertyChange(new ChangedProperty(nameof(VolumeUpButtonState), value.ToString()));
                }
            }
        }

        private EButtonState _previousButtonState;

        /// <summary>
        /// State of the previous button
        /// </summary>
        public EButtonState PreviousButtonState
        {
            get => _previousButtonState;
            set
            {
                if (_previousButtonState != value)
                {

                    _previousButtonState = value;
                    OnPropertyChange(new ChangedProperty(nameof(PreviousButtonState), value.ToString()));
                }
            }
        }

        private EButtonState _nextButtonState;

        /// <summary>
        /// State of the next button
        /// </summary>
        public EButtonState NextButtonState
        {
            get => _nextButtonState;
            set
            {
                if (_nextButtonState != value)
                {

                    _nextButtonState = value;
                    OnPropertyChange(new ChangedProperty(nameof(NextButtonState), value.ToString()));
                }
            }
        }

        private EButtonState _playPauseButtonState;

        /// <summary>
        /// State of the play/pause button
        /// </summary>
        public EButtonState PlayPauseButtonState
        {
            get => _playPauseButtonState;
            set
            {
                if (_playPauseButtonState != value)
                {

                    _playPauseButtonState = value;
                    OnPropertyChange(new ChangedProperty(nameof(PlayPauseButtonState), value.ToString()));
                }
            }
        }

        public event EventHandler<ChangedProperty> OnPropertyChanged;

        private void OnPropertyChange(ChangedProperty changedProperty)
        {
            OnPropertyChanged?.Invoke(this, changedProperty);
        }
    }
}
