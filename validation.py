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
    import pandas as pd
    import numpy as np
    import datetime

    from scipy.sparse import csr_matrix

    from sklearn.model_selection import train_test_split
    import feather

    train = train[train["listen_type"] == 1]

    temp = train.groupby(["user_id"]).size().reset_index()
    temp.columns = ["user_id", "n"]
    train = pd.merge(train, temp)

    for i in range(1,4):
        feather.write_dataframe(train[train["n"] > i].groupby(["user_id"]).apply(lambda x: x.iloc[-i]), path_to_folds + "/test_{}.feather".format(i))
        feather.write_dataframe(train[train["n"] > i].groupby(["user_id"]).apply(lambda x: x.iloc[:-i]), path_to_folds + "/train_{}.feather".format(i))
