from sklearn.metrics import roc_auc_score
import numpy as np
import pandas as pd

def train_and_validate(df, x_cols, y_col, clf, train_validation_split_function):
	train_i, validation_i = train_validation_split_function(df)
	
	Y_train = df.loc[train_i][y_col]
	X_train = df.loc[train_i][x_cols]

	Y_validation = df.loc[validation_i][y_col]
	X_validation = df.loc[validation_i][x_cols]
	
	clf = clf.fit(X_train, Y_train)
	
	Y_predicted = clf.predict_proba(X_validation)[:,1]
	return clf, roc_auc_score(np.array(Y_validation), Y_predicted)
	

def do_submission(df, x_cols, path_to_submission, clf):
	X_test = df[x_cols]
	Y_test = clf.predict_proba(X_test)[:,1]

	submission = pd.DataFrame(columns=['sample_id', 'is_listened'])
	submission['is_listened'] = Y_test
	submission['sample_id'] = df.index

	submission.to_csv(path_to_submission, encoding='utf-8', index=False)
	