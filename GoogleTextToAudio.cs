using System;
using System.Activities;
using System.ComponentModel;
using Google.Cloud.TextToSpeech.V1;
using System.IO;



namespace Google_Text_To_Audio_Activity
{
    public class Text_To_Audio_File : CodeActivity
    {
        public string EnvironmentVariableName = "GOOGLE_APPLICATION_CREDENTIALS";

        public SsmlVoiceGender _voiceGender;
        [Category("Input")]
        [Description("The gender of the voice")]
        public SsmlVoiceGender VoiceGender { get => _voiceGender; set => _voiceGender = value; }

        [Category("Input")]
        [Description("en-US, fr-FR, de-DE, ja-JP")]
        [RequiredArgument]
        public InArgument<string> LanguageCode { get; set; }

        /// input credential file path
        [Category("Input")]
        [Description("the file path of the JSON file that contains your service account key")]
        [RequiredArgument]
        public InArgument<string> ServiceAccountFile { get; set; }

        /// input text
        [Category("Input")]
        [Description("text to convert")]
        [RequiredArgument]
        public InArgument<string> Text { get; set; }

        // Select the type of audio file you want returned.
        [Category("Input")]
        [Description("the type of audio file you want returned")]
        public AudioEncoding AudioEncoder { get; set; }

        [Category("Input")]
        [Description("file name.file extension")]
        [RequiredArgument]
        public InArgument<string> AudioFilePath { get; set; }


        protected override void Execute(CodeActivityContext context)
        {
            /// set credential file path to GOOGLE_APPLICATION_CREDENTIALS environment variable. 
            string path = @ServiceAccountFile.Get(context);
            Environment.SetEnvironmentVariable(EnvironmentVariableName, path);


            // Instantiate a client
            var client = TextToSpeechClient.Create();

            // Set the text input to be synthesized.
            var input = new SynthesisInput
            {
                //Text = "text to convert in audio file"
                Text = Text.Get(context),
            };

            // Build the voice request
            var voice = new VoiceSelectionParams
            {
                LanguageCode = LanguageCode.Get(context),
                SsmlGender = VoiceGender,
            };

            var config = new AudioConfig
            {
                AudioEncoding = AudioEncoder,
            };
            // Perform the Text-to-Speech request, passing the text input
            // with the selected voice parameters and audio file type
            var response = client.SynthesizeSpeech(new SynthesizeSpeechRequest
            {
                Input = input,
                Voice = voice,
                AudioConfig = config
            });

            // Write the binary AudioContent of the response to an audio file.
            using (Stream output = File.Create(AudioFilePath.Get(context)))
            {
                response.AudioContent.WriteTo(output);

            }
        }
    }
    /// <summary>
    /// *********************************************************************************************
    /// </summary>
    public class SSML_To_Audio_File : CodeActivity
    {
        public string EnvironmentVariableName = "GOOGLE_APPLICATION_CREDENTIALS";

        public SsmlVoiceGender _voiceGender;
        [Category("Input")]
        [Description("The gender of the voice")]
        public SsmlVoiceGender VoiceGender { get => _voiceGender; set => _voiceGender = value; }

        [Category("Input")]
        [Description("en-US, fr-FR, de-DE, ja-JP")]
        [RequiredArgument]
        public InArgument<string> LanguageCode { get; set; }

        /// input credential file path
        [Category("Input")]
        [Description("the file path of the JSON file that contains your service account key")]
        [RequiredArgument]
        public InArgument<string> ServiceAccountFile { get; set; }

        /// input text
        [Category("Input")]
        [Description("SSML text to convert")]
        [RequiredArgument]
        public InArgument<string> SSML { get; set; }

        // Select the type of audio file you want returned.
        [Category("Input")]
        [Description("the type of audio file you want returned")]
        public AudioEncoding AudioEncoder { get; set; }

        [Category("Input")]
        [Description("file name.file extension")]
        [RequiredArgument]
        public InArgument<string> AudioFilePath { get; set; }


        protected override void Execute(CodeActivityContext context)
        {
            /// set credential file path to GOOGLE_APPLICATION_CREDENTIALS environment variable. 
            string path = @ServiceAccountFile.Get(context);
            Environment.SetEnvironmentVariable(EnvironmentVariableName, path);


            // Instantiate a client
            var client = TextToSpeechClient.Create();

            // Set the text input to be synthesized.
            var input = new SynthesisInput
            {
                //Text = "text to convert in audio file"
                Ssml = SSML.Get(context),
            };

            // Build the voice request
            var voice = new VoiceSelectionParams
            {
                LanguageCode = LanguageCode.Get(context),
                SsmlGender = VoiceGender,
            };

            var config = new AudioConfig
            {
                AudioEncoding = AudioEncoder,
            };
            // Perform the Text-to-Speech request, passing the text input
            // with the selected voice parameters and audio file type
            var response = client.SynthesizeSpeech(new SynthesizeSpeechRequest
            {
                Input = input,
                Voice = voice,
                AudioConfig = config
            });

            // Write the binary AudioContent of the response to an audio file.
            using (Stream output = File.Create(AudioFilePath.Get(context)))
            {
                response.AudioContent.WriteTo(output);

            }
        }
    }
}
