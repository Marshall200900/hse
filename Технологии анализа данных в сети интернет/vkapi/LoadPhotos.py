# Импортируем нужные модули
from urllib.request import urlretrieve
import vk, os, time, math

# Авторизация

# login = ''
# password = ''
# vk_id = ''

# session = vk.AuthSession(app_id=vk_id, user_login=login, user_password=password) 
# vkapi = vk.API(session, v='5.81')

session = vk.Session(access_token='8ad739e5417b57f55e87601627432331fba8fbfee4e9794133290021743513c5f275843568f5d8cce5c8c')
vkapi = vk.API(session, v='5.81')

url = "https://vk.com/album-132_47581240"
# Разбираем ссылку
owner_id = url.split('/')[-1].split('_')[0].replace('album', '')

albums = vkapi.photos.getAlbums(owner_id=owner_id)



if not os.path.exists('saved'):
    os.mkdir('saved')

time_now = time.time() # время старта

counter_total = 0
broken_total = 0


for album in albums['items']:
    photo_folder = 'saved/album{0}_{1}'.format(owner_id, album['id'])
    if not os.path.exists(photo_folder):
        os.mkdir(photo_folder)
    photos_count = album['size']

    

    counter = 0 # текущий счетчик
    breaked = 0 # не загружено из-за ошибки
    prog = 0 # процент загруженных
    
    for j in range(math.ceil(photos_count / 1000)): # Подсчитаем&nbsp;сколько раз нужно получать список фото, так как число получится не целое - округляем в большую сторону
        photos = vkapi.photos.get(owner_id=owner_id, album_id=album['id'], count=1000, offset=j*1000) #&nbsp;Получаем список фото
        for photo in photos["items"]:
            counter += 1
            url = photo["sizes"][-1]["url"] # Получаем адрес изображения
            print('Загружаю фото № {} из {} альбома {}. Прогресс: {} %'.format(counter, photos_count, album['id'], prog))
            prog = round(100/photos_count*counter,2)
            try:
                urlretrieve(url, photo_folder + "/" + os.path.split(url)[1].split('?')[0]) # Загружаем и сохраняем файл
            except Exception:
                print(url)
                print('Произошла ошибка, файл пропущен.')
                breaked += 1
    
    counter_total += counter
    broken_total += breaked

time_for_dw = time.time() - time_now
print("\nВ очереди было {} файлов. Из них удачно загружено {} файлов, {} не удалось загрузить. Затрачено времени: {} сек.". format(counter_total, counter_total-broken_total, broken_total, round(time_for_dw,1)))
