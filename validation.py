import pandas as pd
import numpy as np
import datetime

from scipy.sparse import csr_matrix

from sklearn.model_selection import train_test_split
import feather

def split_train_validation_by_last_record(train):
	train = train.sort_values(by='ts_listen', axis=0)
	train['index'] = train.index
	validation = train.groupby(['user_id'], as_index=False).last()
	validation_indexes = set(validation['index'])
	train_indexes = set(set(train.index) - validation_indexes)
	return train_indexes, validation_indexes
	
	
def split_train_validation_by_last_record_and_flow(train):
	train = train.sort_values(by='ts_listen', axis=0)
	train['index'] = train.index
	validation = train[train['listen_type'] == 1].groupby(['user_id']).last()
	
	ls = []
	for user_id,group in train.groupby('user_id'):
		if(user_id in validation.index):
			ls.append(group[group['ts_listen'] > validation.loc[user_id]['ts_listen']]['index'])
			
	indexes_after_test_entry = set([index for index_ls in ls for index in index_ls])
	validation_indexes = set(validation['index'])

	train_indexes = set((set(train.index) - indexes_after_test_entry) - validation_indexes)
	return train_indexes, validation_indexes
	
	
def only_flow_split_train_validation_by_last_record(train):
	train = train[train['listen_type'] == 1].sort_values(by='ts_listen', axis=0)
	train['index'] = train.index
	validation = train.groupby(['user_id'], as_index=False).last()
	validation_indexes = set(validation['index'])
	train_indexes = set(set(train.index) - validation_indexes)
	return train_indexes, validation_indexes


def generate_validation_folds(train, path_to_folds):
    temp = train.groupby(['user_id'])["listen_type"].sum().reset_index()
    temp.columns = ['user_id', 'n']
    train = pd.merge(train, temp, how='left', on='user_id')

    train = train.sort_values(["user_id", "ts_listen"])

    def prepare_train(df, fold):
        print("Fold {}".format(fold))
        df_temp = df.copy()
        temp = df[(df["listen_type"] == 1) & (df["n"] >= fold)].groupby(['user_id']).apply(lambda x: x.iloc[-fold])["ts_listen"].reset_index()
        temp.columns = ["user_id", "ts_listen_last"]
        df_temp = pd.merge(df_temp, temp, how='left', on='user_id')
        df_temp = df_temp[df_temp['ts_listen'] <= df_temp['ts_listen_last']]
        feather.write_dataframe(df_temp[df_temp["n"] >= fold].groupby(["user_id"]).apply(lambda x: x.iloc[-1]), path_to_folds + "/test_{}.feather".format(i))
        feather.write_dataframe(df_temp[df_temp["n"] >= fold].groupby(["user_id"]).apply(lambda x: x.iloc[:-1]), path_to_folds + "/train_{}.feather".format(i))

    for i in range(1,2):
        prepare_train(train, i)
