import pandas as pd
import feather
import six.moves.cPickle
import lightgbm


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

lgb_train = lightgbm.Dataset(train[cols], train["is_listened"])
model_lgm = lightgbm.train(params, lgb_train, 550)

lgbm_preds = model_lgm.predict(test[cols])

submition = pd.read_csv("../data/sample_submission_kaggle.csv")

submition["is_listened"] = lgbm_preds
submition.to_csv("../submissions/submit_lgm_stas_new_features.csv", index=False)





