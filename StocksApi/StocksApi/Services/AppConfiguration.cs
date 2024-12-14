using Library.Interfaces;

namespace StocksApi.Services
{
	public class AppConfiguration : IAppConfiguration
	{
        private readonly IConfiguration _configuration;

        public AppConfiguration(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public T Get<T>(string section)
		{
			var value = _configuration.GetRequiredSection(section).Get<T>();

			return value!;
		}

		public T Get<T>(params string[] sections)
		{
			if (sections.Length == 0)
				throw new Exception("Please enter sections");

			var sectionsList = sections.ToList();
			IConfigurationSection configurationSection = _configuration.GetRequiredSection(sectionsList[0]);
			sectionsList.RemoveAt(0);

            do
            {
				var currentSection = sectionsList[0];
                configurationSection = configurationSection.GetRequiredSection(currentSection);
				sectionsList.Remove(currentSection);
            } while (sectionsList.Count > 1);

			var value = configurationSection.Get<T>();

			return value!;
		}
	}
}