import time

import feather
import pandas as pd
import numpy as np
import six


class SemenovEncoding:
    def __init__(self, C=10):
        self.C = C
        self.cpu_k = 3
        self.global_mean = 0
        self.features = 'all'
        self.cat_columns = []
        self.y = 0
        self.values = dict()

    def fit(self, data, y, features='all'):

        self.y = y
        self.cat_columns = sorted([i for i in data.columns if data[i].dtype == 'O'])
        if features == 'all':
            self.features = self.cat_columns
        else:
            self.features = features

        self.global_mean = np.mean(y)

        f = {'y': ['size', 'mean']}

        for col in self.features:
            print("{} is processing...".format(col))
            self.values[col] = dict()
            temp = pd.DataFrame({'y': y, col: data[col]}).groupby([col]).agg(f)

            self.values[col] = (
            (temp['y']['mean'] * temp['y']['size'] + self.global_mean * self.C) / (temp['y']['size'] +
                                                                                   self.C)).to_dict()
        return self.values

    def fit_transform(self, data, y, features='all', inplace=True):

        self.fit(data, y, features)
        return self.transform(data, inplace=inplace)

    def transform(self, data, inplace=True):
        import warnings

        if inplace:
            for col in self.values:
                if col in data.columns:
                    temp = pd.DataFrame.from_dict(self.values[col], orient='index').reset_index()
                    temp.columns = [col, 'value']
                    data = pd.merge(data, temp, how='left').fillna(self.global_mean)
                    data[col] = data['value']
                    del data['value']
                    data[col] = data[col].astype('float32')

                else:
                    warnings.warn('Column ' + col + ' is missed in this dataset.')
        else:
            new_data = data.copy()
            for col in self.values:
                if col in new_data.columns:
                    temp = pd.DataFrame.from_dict(self.values[col], orient='index').reset_index()
                    temp.columns = [col, 'value']
                    new_data = pd.merge(new_data, temp, how='left').fillna(self.global_mean)
                    new_data[col] = new_data['value']
                    del new_data['value']
                    new_data[col] = new_data[col].astype('float32')

                else:
                    warnings.warn('Column ' + col + ' is missed in this dataset.')
            return new_data


train = pd.read_csv('../data/train.csv')
test = pd.read_csv('../data/test.csv')
#cum_train = pd.read_csv('../data/cumsum.train.csv')
#cum_test = pd.read_csv('../data/cumsum.test.csv')

test['is_listened'] = 0
train['sample_id'] = -1
df = pd.concat([train, test])

#df = pd.concat([df, pd.concat([cum_train, cum_test])], axis=1)

#df["new_index"] = np.arange(len(df))
#df.sort_values(['user_id', 'ts_listen'], inplace=True)
#df_2 = df[['user_id', 'ts_listen', 'media_duration']].shift()

#df = pd.concat([df, df_2.rename(columns={'user_id': 'prev_user_id', 'ts_listen': 'prev_ts_listen',
#                                         'media_duration': 'prev_media_duration'})], axis=1)

#df['dif_time'] = df['ts_listen'] - df['prev_ts_listen']
#df['dif_media_duration'] = df['prev_media_duration'] - df['dif_time']
#df['per_dif_media_duration'] = df['dif_time'] / df['prev_media_duration']
#df.ix[df['user_id'] != df['prev_user_id'], ['dif_time', 'dif_media_duration', 'prev_media_duration']] = -999
#df.sort_values(["new_index"],inplace=True)
#del df_2, df['prev_ts_listen'], df['prev_user_id'], df["new_index"]

df['temp'] = '_'
print('User features')
#user features
df['user_id_genre'] = df[["user_id", 'temp', "genre_id"]].astype(str).sum(axis=1)
df['user_id_media_id'] = df[["user_id", 'temp', "media_id"]].astype(str).sum(axis=1)
df['user_id_artist_id'] = df[["user_id", 'temp', "artist_id"]].astype(str).sum(axis=1)
df['user_id_album_id'] = df[["user_id", 'temp', "album_id"]].astype(str).sum(axis=1)

df['user_id_listen_type'] = df[["user_id", 'temp', "listen_type"]].astype(str).sum(axis=1)
df['user_age_user_gender'] = df[["user_age", 'temp', "user_gender"]].astype(str).sum(axis=1)
df['user_age_media_id'] = df[["user_age", 'temp', "media_id"]].astype(str).sum(axis=1)
df['user_age_artist_id'] = df[["user_age", 'temp', "artist_id"]].astype(str).sum(axis=1)
df['user_age_album_id'] = df[["user_age", 'temp', "album_id"]].astype(str).sum(axis=1)

df['user_id_platform_family'] = df[["user_id", 'temp', "platform_family"]].astype(str).sum(axis=1)
df['user_id_platform_name'] = df[["user_id", 'temp', "platform_name"]].astype(str).sum(axis=1)
df['user_id_context_type'] = df[["user_id", 'temp', "context_type"]].astype(str).sum(axis=1)


print("Copy features")
#for col in ['genre_id', 'context_type', 'user_id', 'album_id', 'media_id', 'artist_id']:
#    df["old_{}".format(col)] = df[col].copy()

df.drop(['temp'], axis=1, inplace=True)

train = df.iloc[:len(train)]
test = df.iloc[len(train):]
del df


def sem_proc(train_indexes, val_indexes, df, y, columns_for_encoding):
    se = SemenovEncoding(C=10)
    start = time.time()
    se.fit(df.iloc[train_indexes], y.iloc[train_indexes], features=columns_for_encoding)
    end = time.time()
    print(end - start)
    new_val = se.transform(df.iloc[val_indexes], inplace=False)
    start = time.time()
    print(start - end)
    return new_val

skf = six.moves.cPickle.load(open("../folds/skf_split", "rb"))

target = train.is_listened.copy()
cols = ['genre_id', 'context_type', 'user_id', 'album_id', 'media_id', 'artist_id',
        'user_id_genre', 'user_id_media_id', 'user_id_artist_id', 'user_id_album_id',
        'user_id_listen_type', 'user_age_user_gender', 'user_age_media_id', 'user_age_artist_id',
        'user_age_album_id', 'user_id_platform_family',
        'user_id_platform_name', 'user_id_context_type', 
        'context_type_platform_family', 'platform_family_platform_name',
        'user_age_listen_type']

new_df = []
for train_f, val_f in skf:
    new_df.append(sem_proc(train_f, val_f, train, target, cols))
df = pd.concat(new_df, axis=0)
df.sort_index(inplace=True)

feather.write_dataframe(df, "../folds/train_ids_user_2.feather")

se = SemenovEncoding(C=10)
se.fit(train, target, features=cols)
new_test = se.transform(test, inplace=False)

feather.write_dataframe(new_test, "../folds/test_ids_user_2.feather")


