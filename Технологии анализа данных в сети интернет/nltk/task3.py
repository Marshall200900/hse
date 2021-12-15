import nltk
import string
from os import path
from math import log
from nltk.corpus import stopwords
from nltk.probability import FreqDist

number_of_key_words = 10

fileNames = ['text1.txt', 'text2.txt', 'text3.txt']


norm_tokens = {}
word_entries = {}
words_data = {}

def fetchWords(filename):
    data = readFile(filename)
    
    tokens = nltk.word_tokenize(data)
    # exclude punctuation
    punctuation = string.punctuation + '—–«»”“'
    tokens = [i for i in tokens if (i not in punctuation)]

    # exclude stopwords
    stop_words = stopwords.words('russian')
    tokens = [i for i in tokens if (i not in stop_words)]

    # normalize using stemmer
    stemmer = nltk.stem.SnowballStemmer('russian') # http://snowball.tartarus.org/algorithms/russian/stemmer.html
    normalized_tokens = [stemmer.stem(t) for t in tokens]

    return normalized_tokens


def readFile(filename):
    if not path.exists(filename):
        print(f'Файл {filename} не найден')
        return
    
    # read file
    f = open(filename, 'r', encoding="utf8")
    output = f.read()
    f.close()  
    return output



def weight_counter(filename, word_stat):
    weights = {}
    total_words = len(fetchWords(readFile(filename))) # total words amount in a document
    for word in word_stat:
        if word_entries.get(word[0]) is None:            
            word_entries[word[0]] = 1
        else:
            word_entries[word[0]] += 1                    
        weights[word[0]] = word[1]/total_words    
    return weights
            
for fileName in fileNames:
    norm_tokens[fileName] = fetchWords(fileName)
    
    # sort
    fdist = FreqDist(norm_tokens[fileName])
    sorted_list = sorted(fdist.items(), key=lambda x: x[1], reverse=True) #tuple: (str)WORD, (int)FREQUENCY        
    words_data[fileName] = weight_counter(fileName, sorted_list) # (str)WORD: (float)WEIGHT

f = open('t3res.txt', 'w')    
   
for fileName in fileNames:
    i = 0 # limiter
    nl = '\n' # separator
    
    words = words_data[fileName].keys()
    f.write(f'Анализ документа {fileName}:{nl}Выборка ограничена {number_of_key_words} ключевыми словами{nl}')
    
    for word in words:
        TF = float(words_data[fileName].get(word))
        IDF = log(1+(len(fileNames)/int(word_entries[word])))
        
        f.write(f'{nl}Словоформа: {word}{nl}Частота в документе {fileName}: {TF}{nl}Встречено в {word_entries[word]}/{len(fileNames)} документов{nl}IDF: {IDF}{nl}TF-IDF: {TF*IDF}{nl}')

        i+=1
        if i == number_of_key_words:
            break
    
    f.write('\n' + fileName + ': END OF FILE.\n')

print('Finished.')