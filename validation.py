def split_train_test_by_last_record(train):
	train = train.sort_values(by='ts_listen', axis=0)
	train['index'] = train.index
	test = train.groupby(['user_id'], as_index=False).last()
	test_indexes = set(test['index'])
	train_indexes = set(set(train.index) - test_indexes)
	return train_indexes, test_indexes
	
def split_train_test_by_last_record_and_flow(train):
	train = train.sort_values(by='ts_listen', axis=0)
	train['index'] = train.index
	test = train[train['listen_type'] == 1].groupby(['user_id']).last()
	
	ls = []
	for user_id,group in train.groupby('user_id'):
		if(user_id in test.index):
			ls.append(group[group['ts_listen'] > test.loc[user_id]['ts_listen']]['index'])
			
	indexes_after_test_entry = set([index for index_ls in ls for index in index_ls])
	test_indexes = set(test['index'])

	train_indexes = set((set(train.index) - indexes_after_test_entry) - test_indexes)
	return train_indexes, test_indexes