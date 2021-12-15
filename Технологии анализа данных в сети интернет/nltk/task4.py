import nltk
import string
import operator
from os import path, write
from nltk.corpus import stopwords
from nltk.probability import FreqDist

#nltk.download('punkt') # necessary shnyaga for sentence analysis
#nltk.download('stopwords')

COMPRESSION_RATIO = 0.1 # percent value
sentences_data = {} # (str)SENTENCE: (float)WEIGHT
sentence_weights = [] # list of all sntence weights in order

fileName = 'levtolstoy.txt'

# retrieves text data from a text file
def readFile(filename):
    if not path.exists(filename):
        print(f'Файл {filename} не найден')
        return
    
    # read file
    f = open(filename, 'r', encoding="utf8")
    output = f.read()
    f.close()  
    return output    
# fetches infinitive forms of all words met in a document
def fetchWords(filename):
    data = readFile(filename) #txt file text
    
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
# calculates words weight in a document (TF)
def weight_counter(filename, word_stat):
    weights = {} # (str)WORD: (float)WEIGHT
    total_words = len(readFile(filename).split()) # total words amount in a document
    for word in word_stat:                       
        weights[word[0]] = word[1]/total_words    
    return weights
# changes words to their infinitives keeping original sentence structure
def fetchSentences(filename):
    sentences = nltk.sent_tokenize(readFile(fileName)) # unchanged sentenced in the document
    norm_sentences = ""
    stemmer = nltk.stem.SnowballStemmer('russian') # http://snowball.tartarus.org/algorithms/russian/stemmer.html
    for sentence in sentences:
        words = str(sentence).lower().split()
        normalized_tokens = [stemmer.stem(w) for w in words]
        for token in normalized_tokens:
            norm_sentences += token + " "
    return norm_sentences

    print(normalized_sentences)
    return
# calculates sentence weight as sum of all weights of words in
def weight_sentences (sentences, word_weights):   
    for sentence in sentences:
        words = str(sentence).lower().split()
        weight = 0
        for word in words:
            for inf_word in word_weights.keys():
                if str(word).__contains__(inf_word):
                    weight += word_weights[inf_word]                 
                    break     
        if weight != 0:       
            sentences_data[sentence] = weight
        else:
            print(f'Ошибка в вычислении веса предложения {str(sentence)}')
            

norm_tokens = fetchWords(fileName) # infinitives from the document text
sentences = nltk.sent_tokenize(readFile(fileName)) # unchanged sentenced in the document

# sort
fdist = FreqDist(norm_tokens)
sorted_list = sorted(fdist.items(), key=lambda x: x[1], reverse=True) #tuple: (str)WORD, (int)FREQUENCY        
words_data = weight_counter(fileName, sorted_list) # (str)WORD: (float)WEIGHT

weight_sentences(sentences, words_data)
sorted_sentences = dict(sorted(sentences_data.items(), key=operator.itemgetter(1),reverse=True))

output_amount = int(round(len(sentences)*COMPRESSION_RATIO))

f = open('task4_output.txt', 'w')   
nl = '\n'
f.write(f'Аннотация к тексту {fileName}:{nl}Всего предложений - {len(sentences)}, коэффициент сжатия: {COMPRESSION_RATIO*100}%{nl}Объем аннотации - {output_amount} предложений.{nl}Приятного чтения!{nl}') 

i = 1
for sent in sorted_sentences:
    if (i <= output_amount):
        f.write(sent)
        i+=1
    else:
        break
      
print('Результат работы программы в файле task4_output.txt')