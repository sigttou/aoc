(ns aoc-2023.day-20
  (:require [aoc-2023.helpers :as helpers]
            [clojure.math.numeric-tower :as math]
            [clojure.string :as string]))

(def input-file-path "inputs/day_20/input")
(def sample-file-path "inputs/day_20/sample-1")
(def sample-2-file-path "inputs/day_20/sample-2")

(defn parse-input
  [filename]
  (let [entries (map #(string/split % #" -> ")
                     (string/split (slurp filename) #"\n"))
        modules (reduce (fn [out entry]
                          (let [[mtype name]
                                (if (= \% (first (first entry)))
                                  [:flip (apply str (rest (first entry)))]
                                  (if (= \& (first (first entry)))
                                    [:conj (apply str (rest (first entry)))]
                                    [:broadcast (first entry)]))
                                outputs (string/split (second entry) #", ")]
                            (assoc out name {:type mtype
                                             :outputs outputs})))
                        {}
                        entries)]
    modules))

(defn get-inputs
  [modules]
  (reduce (fn [outm [name module]]
            (reduce (fn [out output]
                      (assoc out output (conj (get out output) name)))
                    outm
                    (:outputs module)))
          {}
          modules))

(defn get-history
  [modules inputs]
  (reduce (fn [outm [name module]]
            (if (= (:type module) :flip)
              (assoc outm name false)
              (if (= (:type module) :conj)
                (assoc outm name (reduce #(assoc %1 %2 false)
                                         {} (get inputs name)))
                outm)))
          {}
          modules))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [modules (parse-input filename)
         inputs (get-inputs modules)]
     (loop [hist (get-history modules inputs)
            todo [[nil "broadcaster" false]]
            cnt 0
            highcnt 0
            lowcnt 0]
       (if (< cnt 1000)
         (if (empty? todo)
           (recur hist [[nil "broadcaster" false]] (inc cnt) highcnt lowcnt)
           (let [[new-hcnt new-lcnt new-hist new-todo]
                 (reduce
                  (fn [[hcnt lcnt lhist ltodo] [src name high]]
                    (concat
                     (if high
                       [(inc hcnt) lcnt]
                       [hcnt (inc lcnt)])
                     (if-let [module (get modules name)]
                       (case (:type module)
                         :flip (if high
                                 [lhist ltodo]
                                 (let [state (get lhist name)]
                                   [(assoc lhist name (not state))
                                    (concat ltodo
                                            (mapv #(identity
                                                    [name % (not state)])
                                                  (:outputs module)))]))
                         :conj (let [states (get lhist name)
                                     nstates (assoc states src high)
                                     signal (if (some false? (vals nstates))
                                              true
                                              false)]
                                 [(assoc lhist name nstates)
                                  (concat ltodo (mapv #(identity
                                                        [name % signal])
                                                      (:outputs module)))])
                         :broadcast [lhist (concat ltodo
                                                   (mapv #(identity
                                                           [name % high])
                                                         (:outputs module)))])
                       [lhist ltodo])))
                  [highcnt lowcnt hist []]
                  todo)]
             (recur new-hist new-todo cnt new-hcnt new-lcnt)))
       (* highcnt lowcnt))))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [modules (parse-input filename)
         inputs (get-inputs modules)
         sources (get inputs (first (get inputs "rx")))]
     (loop [hist (get-history modules inputs)
            todo [[nil "broadcaster" false]]
            cnt 1
            counts (into {} (map #(identity [% 0]) sources))]
       (if (some zero? (vals counts))
         (if (empty? todo)
           (recur hist [[nil "broadcaster" false]] (inc cnt) counts)
           (let [[new-counts new-hist new-todo]
                 (reduce
                  (fn [[lcounts lhist ltodo] [src name high]]
                    (concat
                     [(if (and (some #{name} sources)
                               (not high)
                               (= 0 (get lcounts name)))
                        (assoc lcounts name cnt)
                        lcounts)]
                     (if-let [module (get modules name)]
                       (case (:type module)
                         :flip (if high
                                 [lhist ltodo]
                                 (let [state (get lhist name)]
                                   [(assoc lhist name (not state))
                                    (concat ltodo
                                            (mapv #(identity
                                                    [name % (not state)])
                                                  (:outputs module)))]))
                         :conj (let [states (get lhist name)
                                     nstates (assoc states src high)
                                     signal (if (some false? (vals nstates))
                                              true
                                              false)]
                                 [(assoc lhist name nstates)
                                  (concat ltodo (mapv #(identity
                                                        [name % signal])
                                                      (:outputs module)))])
                         :broadcast [lhist (concat ltodo
                                                   (mapv #(identity
                                                           [name % high])
                                                         (:outputs module)))])
                       [lhist ltodo])))
                  [counts hist []]
                  todo)]
             (recur new-hist new-todo cnt new-counts)))
       (reduce math/lcm (vals counts)))))))

(defn run
  []
  (println (part-one))
  (println (part-two)))