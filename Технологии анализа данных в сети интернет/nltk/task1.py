import nltk
import string
from nltk.corpus import stopwords
from nltk.probability import FreqDist
import pymorphy2

# read file
f = open('Bible_txt.txt', 'r')
tokens = nltk.word_tokenize(f.read())
f.close()

# exclude punctuation
punctuation = string.punctuation + '—–«»”“'
tokens = [i for i in tokens if (i not in punctuation)]

# exclude stopwords
stop_words = stopwords.words('russian')
tokens = [i for i in tokens if (i not in stop_words)]

# normalize
morph = pymorphy2.MorphAnalyzer()
normalized_tokens = [morph.parse(t)[0].normal_form for t in tokens]

# sort
fdist = FreqDist(normalized_tokens)
sorted_list = sorted(fdist.items(), key=lambda x: x[1], reverse=True)

# write to file
f = open('result.txt', 'w')
for pair in sorted_list:
    f.write(str(pair[0]) + ',' + str(pair[1]) + '\n')