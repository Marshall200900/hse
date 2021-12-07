# tf-idf
import nltk
import string
from nltk.corpus import stopwords
from nltk.probability import FreqDist
import pymorphy2


def getOccurencesDict(filename):
    # read file
    f = open(filename, 'r')
    tokens = nltk.word_tokenize(f.read())
    f.close()

    # exclude punctuation
    punctuation = string.punctuation + '–«»”“'
    tokens = [i for i in tokens if (i not in punctuation)]

    # exclude stopwords
    stop_words = stopwords.words('russian')
    tokens = [i for i in tokens if (i not in stop_words)]

    # normalize using stemmer
    stemmer = nltk.stem.SnowballStemmer('russian')
    normalized_tokens = [stemmer.stem(t) for t in tokens]

    # sort
    fdist = FreqDist(normalized_tokens)
    sorted_list = sorted(fdist.items(), key=lambda x: x[1], reverse=True)
    return sorted_list


array_of_dicts = []
for i in range(3):
    array_of_dicts.append(getOccurencesDict('task{}.txt'.format(i)))



