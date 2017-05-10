Model 1: lightgbm 150 итераций на cols = ['genre_id', 'album_id', 'media_id', 'artist_id',
'context_type', 'user_id', 'release_date', 'platform_name',
'platform_family', 'media_duration', 'listen_type', 'user_gender',
'user_age', 'user_id_genre', 'user_id_media_id',
'user_id_artist_id', 'user_id_album_id',
'user_id_listen_type', 'user_age_user_gender', 'user_age_media_id',
'user_age_artist_id', 'user_age_album_id', 'cumsum_listen_type',
'cumsum_listen_type_1', 'cumsum_listen_type_0', 'cumsum_is_listened',
'cumsum_is_listened_1', 'cumsum_is_listened_0’]

cv = 0.885 <br>
lb = 0.65875 <br>

Model 2: lightgbm 550 итераций на cols = ['genre_id', 'album_id', 'media_id', 'artist_id',
'context_type', 'user_id', 'release_date', 'platform_name',
'platform_family', 'media_duration', 'listen_type', 'user_gender',
'user_age', 'user_id_genre', 'user_id_media_id',
'user_id_artist_id', 'user_id_album_id',
'user_id_listen_type', 'user_age_user_gender', 'user_age_media_id',
'user_age_artist_id', 'user_age_album_id',
'user_id_platform_family', 'user_id_platform_name', 
'user_id_context_type']

cv = 0.847
lb = 0.66411

Best result: Model1 * 0.4 + Model2 * 0.6
LB = 0.67081
