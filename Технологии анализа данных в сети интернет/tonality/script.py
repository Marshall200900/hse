import requests
import pandas as pd
from bs4 import BeautifulSoup as bs
from azure.ai.textanalytics import TextAnalyticsClient
from azure.core.credentials import AzureKeyCredential
import matplotlib.pyplot as plt
import numpy as np


endpoint = "https://coolresource.cognitiveservices.azure.com/"
key = "9e75fe74f62540ebac79e91f695d280c"


headers = {'User-Agent': 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36' }

film_ids = ['tt0302886']
REVIEWS_LINK = 'https://www.imdb.com/title/{}/reviews?ref_=tt_ov_rt'


reviews_ratings = []
reviews_score_positive = []
reviews_texts = []


class Review:
    review_id = 1
    def __init__(self, title = '', rating = '', content = '', score_positive = 0.0, score_neutral = 0.0, score_negative = 0.0, sentiment = 0.0):
        self.id = Review.review_id
        self.title = title
        self.rating = rating
        self.content = content
        self.score_positive = score_positive
        self.score_neutral = score_neutral
        self.score_negative = score_negative
        self.sentiment = sentiment
        Review.review_id += 1
        
    def output(self):
        print('\nREVIEW#{}\nRating: {}\nTitle: {}\nContent: {}\nSentiment: {}\nPositive: {}\nNegative: {}\nNeutral: {}\n'
              .format(f'{self.id}\n',
                      'UNDEFINED' if self.rating is None else f'{self.rating}',
                      'UNDEFINED' if self.title is None else f'"{self.title}"',
                      'UNDEFINED' if self.content is None else f'"{self.content}"',
                      0.0 if self.sentiment is None else f'{self.sentiment}',
                      0.0 if self.score_positive is None else f'{self.score_positive}',
                      0.0 if self.score_negative is None else f'{self.score_negative}',
                      0.0 if self.score_neutral is None else f'{self.score_neutral}'))

# Authenticate the client using your key and endpoint 
def authenticate_client():
    ta_credential = AzureKeyCredential(key)
    text_analytics_client = TextAnalyticsClient(
            endpoint=endpoint, 
            credential=ta_credential)
    return text_analytics_client
          
# Detecting sentiment in text
def sentiment_analysis_example(client, reviews):
    i = 0
    def batch(arr, n=1):
        for ndx in range(0, len(arr), n):
            yield arr[ndx:min(ndx + n, len(arr))]
            
    contents = [rev[0:4096] for rev in reviews]
    
    for x in batch(contents, 3):
        responses = client.analyze_sentiment(documents=x)
        for response in responses:
            reviews[i].sentiment = response.sentiment
            reviews[i].score_positive = response.confidence_scores.positive
            reviews[i].score_neutral = response.confidence_scores.neutral
            reviews[i].score_negative = response.confidence_scores.negative
            i += 1
        
for id in film_ids:
    r = requests.get(REVIEWS_LINK.format(id), headers=headers)
    soup = bs(r.content,"html.parser")
  
    for review_block in soup.find_all("div", {"class":"review-container"}):  
        if (review_block.find_all("span", {"class":"rating-other-user-rating"})):
            reviews_ratings.append(review_block.find_all("span", {"class":"rating-other-user-rating"})[0].find("span").text)
        if review_block.find("div", {"class":"content"}):
            container = review_block.find("div", {"class":"content"})     
        reviews_texts.append(container.find("div", {"class":"text"}).text)

client = authenticate_client()

sentiment_analysis_example(client, reviews_texts)


data = {'rating':[float(rev) for rev in reviews_ratings],
        'sentiment':[rev.score_positive * 10 for rev in reviews_score_positive]}
df = pd.DataFrame(data)
x = np.arange(len(reviews_ratings))
plt.axis([0,len(reviews_ratings),0,10])
plt.plot(x,df)
plt.legend(data, loc=2)
plt.show()
