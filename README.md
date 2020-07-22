# News Scraper Console App

This console app takes a site url from Fox or CNN as a parameter (four examples are already hard coded in Program.cs), scrapes the site for the content of the article, converts the document to a string removing superfluous information, and then makes a fetch call with the content to Cloudmersive's Natural Language Processing Sentiment Analysis API.

### Installation

Along with this repository, you will need to aquire an API key from [Cloudmersive](https://account.cloudmersive.com/).
After aquiring an API key, replace "YOUR_API_KEY" with your newly aquired API key (keep it secret, keep it safe) on line 140 of the Program.cs file.
