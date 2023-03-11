namespace Net7
{
	public class PlayingWithInterfaces
	{
		[Fact]
		public void User_speaks()
		{
			var user = new User();
			var sentence = user.Speak("hi"); // only the method makes it possible to access 
											 // the default interface method from the implementing type
			Assert.Equal("hi", sentence);
		}

		[Fact]
		public void Simon_says()
		{
			var simon = new Simon();
			var sentence = simon.Speak("hi");
			Assert.NotEqual("hi", sentence);
		}

		[Fact]
		public void Parrot_repeats()
		{
			var parrot = new Parrot();
			var sentence = parrot.Speak("hi");
			Assert.Equal("hi, hi!", sentence);
		}

		[Fact]
		public void Parrot_speaker_repeats()
		{
			var speaker = Create();
			var sentence = speaker.Speak("hi");
			Assert.Equal("hi, hi!", sentence);

			static ISpeaker Create()
			{
				return new Parrot();
			}
		}
	}
	public interface ISpeaker
	{
		string Speak(string phrase)
			=> phrase;
	}

	public class User : ISpeaker
	{
	}

	public class Simon : ISpeaker
	{
		string ISpeaker.Speak(string phrase) => $"Simon says: \"{phrase}\"!";
	}

	public class Parrot : ISpeaker
	{
		public string Speak(string phrase) => $"{phrase}, {phrase}!";
	}


	public static class SpeakerExtensions
	{
		public static string Speak<T>(this T self, string phrase) where T : ISpeaker
			=> self.Speak(phrase);
	}
}