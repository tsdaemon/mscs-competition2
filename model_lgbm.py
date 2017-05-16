import pandas as pd
import numpy as np
import feather
import lightgbm as lgb

def crossvalidate_model(path_to_folds, cols, params, verbose=1, iterations=1000, validate_on_both=False):
    
    evs = []
    importance = None
    for i in range(1,2):
        train = feather.read_dataframe(path_to_folds + "/train_{}.feather".format(i))
        test = feather.read_dataframe(path_to_folds + "/test_{}.feather".format(i))
        lgb_train = lgb.Dataset(train[cols], train["is_listened"])
        lgb_test = lgb.Dataset(test[cols], test["is_listened"])
        evals = {}
        validation = lgb_test
        if(validate_on_both):
            validation = [lgb_train,lgb_test]
        model_lgm = lgb.train(params, lgb_train, iterations, valid_sets=validation, verbose_eval=verbose, evals_result=evals)
        key = "valid_1" if(validate_on_both) else "valid_0"
        evs.append(evals[key]["auc"][-1])
        importance = model_lgm.feature_importance()

    return evs,importance


def crossvalidate_model_by_index(df, train_index, validation_index, cols, params, verbose=1, iterations=1000, validate_on_both=False):
    evs = []
    importance = None

    lgb_train = lgb.Dataset(df.loc[train_index][cols], df.loc[train_index]["is_listened"])
    lgb_test = lgb.Dataset(df.loc[validation_index][cols], df.loc[validation_index]["is_listened"])
    evals = {}
    validation = lgb_test
    if (validate_on_both):
        validation = [lgb_train, lgb_test]
    model_lgm = lgb.train(params, lgb_train, iterations, valid_sets=validation, verbose_eval=verbose,
                          evals_result=evals)
    key = "valid_1" if (validate_on_both) else "valid_0"
    evs.append(evals[key]["auc"][-1])
    importance = model_lgm.feature_importance()

    return evs, importance, model_lgm


def create_submission(train, test, path_to_sample_submission, params, cols, path_to_submission, verbose=1, iterations=1000):
    lgb_train = lgb.Dataset(train[cols], train["is_listened"])
    model_lgm = lgb.train(params, lgb_train, iterations, verbose_eval=verbose)

    lgbm_preds = model_lgm.predict(test[cols])
    submission = pd.read_csv(path_to_sample_submission)
    submission["is_listened"] = lgbm_preds
    submission.to_csv(path_to_submission, index=False)
    return model_lgm.feature_importance(), lgbm_preds, model_lgm
    

def feature_score(fs, cols):
    fscore = list(zip(cols, fs))
    fscore.sort(key=lambda x: x[1])
    fscore.reverse()
    return fscore


