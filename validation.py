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