# Импортируем нужные модули
from urllib.request import urlretrieve
from collections import Counter
from datetime import datetime
from dateutil.relativedelta import relativedelta
from time import sleep
import matplotlib.pyplot as plt

import vk, math

# Авторизация

# login = ''
# password = ''
# vk_id = ''

# session = vk.AuthSession(app_id=vk_id, user_login=login, user_password=password) 
# vkapi = vk.API(session, v='5.81')

session = vk.Session(access_token='8ad739e5417b57f55e87601627432331fba8fbfee4e9794133290021743513c5f275843568f5d8cce5c8c')
vkapi = vk.API(session, v='5.81')

def getUsers(group_id, count=1000):
    users = []

    for i in range(math.ceil(count / 1000)):
        
        users.extend(vkapi.groups.getMembers(group_id=group_id, count=1000, sort='id_desc', offset=i*1000)['items'])
        print('got {} users'.format(1000*(i+1)))
        if i % 10 == 0 and i != 0:
            print('sleeping 2s...')
            sleep(2)
    return users

def findIntersectionPercent(group_id1,group_id2):
    users1 = getUsers(group_id1, 1_000)
    print('sleeping 2s...')
    sleep(2)
    users2 = getUsers(group_id2, 1_000)
    intersection = set(users1).intersection(set(users2))
    return len(intersection) / len(users1) * 100

# Meduza Info
meduza_users = vkapi.groups.getMembers(group_id='meduzaproject', fields='sex,bdate,city', count=1000, sort='id_desc')['items']
meduza_sex = [i['sex'] for i in meduza_users]
meduza_cities = list(filter(lambda x: x != None, [ i['city']['title'] if 'city' in i else None for i in meduza_users]))
meduza_bdate = list(filter(lambda x: x != None, [ i['bdate'] if 'bdate' in i and len(i['bdate'].split('.')) == 3 else None for i in meduza_users]))
meduza_ages_years = [relativedelta(datetime.now(), datetime.strptime(i, '%d.%m.%Y')).years for i in meduza_bdate]    
meduza_city_rate = Counter(meduza_cities)

meduza_female_count = meduza_sex.count(1)
meduza_male_count = len(meduza_sex) - meduza_female_count

print('Meduza:')
print('Female percentage: {:.1f}'.format(meduza_female_count / 1000 * 100))
print('Male percentage: {:.1f}'.format(meduza_male_count / 1000 * 100))
print('Top cities: {}'.format(meduza_city_rate.most_common(3)))
print('Average age: {:.1f}'.format(sum(meduza_ages_years) / len(meduza_ages_years)))


# Perm Active Info
perm_users = vkapi.groups.getMembers(group_id='permactive', fields='sex,bdate,city', count=1000, sort='id_desc')['items']
perm_sex = [i['sex'] for i in perm_users]
perm_cities = list(filter(lambda x: x != None, [ i['city']['title'] if 'city' in i else None for i in perm_users]))
perm_bdate = list(filter(lambda x: x != None, [ i['bdate'] if 'bdate' in i and len(i['bdate'].split('.')) == 3 else None for i in perm_users]))
perm_ages_years = [relativedelta(datetime.now(), datetime.strptime(i, '%d.%m.%Y')).years for i in perm_bdate]    
perm_city_rate = Counter(perm_cities)

perm_female_count = perm_sex.count(1)
perm_male_count = len(perm_sex) - perm_female_count

print('Perm Active:')
print('Female percentage: {:.1f}'.format(perm_female_count / 1000 * 100))
print('Male percentage: {:.1f}'.format(perm_male_count / 1000 * 100))
print('Top cities: {}'.format(perm_city_rate.most_common(3)))
print('Average age: {:.1f}'.format(sum(perm_ages_years) / len(perm_ages_years)))


print('{:.1f} percent of Meduza intersects Perm Active'.format(findIntersectionPercent('meduzaproject', 'permactive')))


fig, ax = plt.subplots(3)

# plot 1
ax[0].set_title('Average age comparison')
ax[0].bar(['Meduza', 'Perm Active'], [sum(meduza_ages_years) / len(meduza_ages_years), sum(perm_ages_years) / len(perm_ages_years)])
# plot 2
ax[1].set_title('Cities of Meduza')
top5 = meduza_city_rate.most_common(5)
labels = [i[0] for i in top5]
values = [i[1] for i in top5]

others = meduza_city_rate
for i in top5:
    others.pop(i[0])

sum_others = sum(list(dict(others).values()))

labels.append('Другие')
values.append(sum_others)

ax[1].pie(values, labels=labels)



ax[2].set_title('Cities of Perm Active')
top5 = perm_city_rate.most_common(3)
labels = [i[0] for i in top5]
values = [i[1] for i in top5]

others = perm_city_rate
for i in top5:
    others.pop(i[0])

sum_others = sum(list(dict(others).values()))

labels.append('Другие')
values.append(sum_others)

ax[2].pie(values, labels=labels)

plt.show()