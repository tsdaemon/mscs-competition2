import feather
import six
import lightgbm
import numpy as np

train = feather.read_dataframe("../folds/train_ids_user_2.feather")
test = feather.read_dataframe("../folds/test_ids_user_2.feather")
skf = six.moves.cPickle.load(open("../folds/skf_split", "rb"))

cols = ['genre_id', 'album_id', 'media_id', 'artist_id',
        'context_type', 'user_id', 'release_date', 'platform_name',
        'platform_family', 'media_duration', 'listen_type', 'user_gender',
        'user_age', 'user_id_genre', 'user_id_media_id',
        'user_id_artist_id', 'user_id_album_id',
        'user_id_listen_type', 'user_age_user_gender', 'user_age_media_id',
        'user_age_artist_id', 'user_age_album_id',
        'user_id_platform_family', 'user_id_platform_name', 
        'user_id_context_type']
       # 'nb_fan', 'nb_album', 'fans', 
       # 'old_genre_id', 'old_context_type', 'old_user_id', 
       # 'old_album_id', 'old_media_id', 'old_artist_id']

params = {
    'application': 'binary',
    'num_leaves': 255,
    'feature_fraction': 0.7,
    'sub_row': 0.7,
    'bagging_freq': 1,
    'max_bin': 255,
    'metric': 'auc',
    'verbose': 0,
    'seed': 42
}

acc = []
for train_f, val_f in skf:
    lgb_train = lightgbm.Dataset(train.iloc[train_f][cols], train.iloc[train_f]["is_listened"])
    lgb_test = lightgbm.Dataset(train.iloc[val_f][cols], train.iloc[val_f]["is_listened"])
    evals = {}
    model_lgm = lightgbm.train(params, lgb_train, 1000, valid_sets=[lgb_test],
                               verbose_eval=1, evals_result=evals, early_stopping_rounds=30)
    acc.append(evals["valid_0"]["auc"][-1])
print(acc)
print(np.mean(acc))

